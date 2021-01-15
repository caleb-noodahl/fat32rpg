using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine.GameObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MerchantRPG.Models.Engine.Multiplayer;

namespace MerchantRPG.Models.Engine.Events
{
    public class TownEvent : GameEvent
    {
        public  List<InventoryItem> _I { get; set; } = new List<InventoryItem>();
        public TownSettings _Towns { get; set; } 

        [JsonConstructor]
        public TownEvent(TownSettings _towns, List<InventoryItem> _i, Context context, bool isComplete, string name, string message, EventAction eventAction)
        {
            _I = _i;
            _Towns = _towns;
            Context = context;
            IsComplete = isComplete;
            Name = name;
            Message = message;
            EventAction = eventAction;
        }

        public TownEvent(string name, TownSettings towns, InventorySettings invSettings)
        {
            Random rand = new Random();
            this.Context = Context.Town;
            this.Name = name == string.Empty ? "Rome" : name;
            this.EventAction = Event;
            _I = invSettings.Default;
            _Towns = towns;
            _Towns.InnName = "The " + Names.Adjective(rand.Next(0, Enum.GetValues(typeof(Names.Adjectives)).Length)) + " " + Names.Things[rand.Next(0, Names.Things.Length)];
        }

        public async Task<PlayerState> Event()
        {
            IsComplete = false;
            Console.WriteLine($"-===========-");
            Console.WriteLine($"Welcome to {this.Name}, " + Party.State.Name);

            while(!IsComplete)
            {
                Console.WriteLine("What is it you'd like to do?");
                Console.WriteLine($"buy or sell or travel or rest or fight or recruit{Environment.NewLine}"); 
                switch(Console.ReadLine().ToLower())
                {
                    case "buy":  DisplayBuyMenu();  break;
                    case "sell": DisplaySellMenu(); break;
                    case "travel": SetTravelContextAndExit(); IsComplete = true; break;
                    case "rest": Rest(); break;
                    case "fight": new MerchantRPG.Models.Engine.Combat.Combat(Party.Members); break;
                    case "recruit": Recruit();  break;
                    default: Console.WriteLine($"I'm afraid that isn't an option...{Environment.NewLine}"); break;
                }
            }
            return Party.State;
        }

        private PlayerState Rest()
        {
            int price = new Random().Next(1, 5) * 10;
            Console.WriteLine("This is " + _Towns.InnName + ", a night here costs " + price + " gold. You staying? (y/n)");
            switch(Console.ReadLine())
            {
                case "y":
                    if (Party.State.Spend(price))
                    {
                        Party.Members.ForEach(member =>
                        {
                            member.DoDamage(-10);
                        });
                        Console.WriteLine("Party healed for 10 at the " + Name + " inn");
                        Client.SendState(Party.Lead.Name + "'s party healed for 10 at the " + Name + " inn");
                    }
                    else
                        Console.WriteLine("You can't afford " + price + "? Get a job you bumb!");
                    break;
                default:
                    break;
            }
            return Party.State;

        }

        private void Recruit()
        {
            Console.WriteLine("You enter the " + Name +  " Barracks. Standing at a makeshift bar in the lobby are " + _Towns.Mercs.Count() + " people, with another at the counter.");
            Console.WriteLine("The person at the counter speaks. 'Hello there. Are you looking to hire? We've got some mighty tough locals looking for some work if you care to take a look.'");
            int mercCount = 1;
            Console.WriteLine("0. Leave");

            _Towns.Mercs.ForEach(m => 
            {
                Console.WriteLine(mercCount + ". " + m.Name + " Lvl:" + m.AbilityLevel + " Dex:" + m.Dexterity + " Str:" + m.Strength + " Int:" + m.Intelligence + " Cost:$" + m.AbilityLevel * 50);
                mercCount++;
            });

            int choice = -1;
            while (choice > _Towns.Mercs.Count() || choice < 0)
            {
                Int32.TryParse(Console.ReadLine(), out choice);
            }

            switch(choice)
            {
                case 0:
                    return;
                    break;
                default:
                    choice -= 1;
                    Character merc = _Towns.Mercs.ElementAt(choice);
                    Console.WriteLine("'Nice to meet you " + Party.State.Name + ", I'm " + merc.Name + ". If you'd like to hire me to enter your service it will cost you $" + merc.AbilityLevel * 50 + ". Would you like me to join you?' (y/n)");
                    if(Console.ReadLine().ToLower() == "y")
                    {
                        if (Party.State.Spend(merc.AbilityLevel * 50))
                        {
                            Party.Members.Add(merc);
                            _Towns.Mercs.Remove(merc);
                            Client.SendState(Party.State.Name + " has hired " + merc.Name + " to join their party.");
                            Client.SendMap(merc.Name + " is no longer at the " + Name + " barracks");
                            Console.WriteLine(merc.Name + " has joined the party!");
                        }
                        else
                            Console.WriteLine("'Doesn't look like you can afford me. Sorry.' " + merc.Name + " returns to their drink.");
                    }
                    break;
            }

        }

        private PlayerState SetTravelContextAndExit()
        {
            Console.WriteLine($"{Environment.NewLine}Which city would you like to embark for?");
            _Towns.TownDefintions.ForEach(x => 
            {
                if(x.Name != this.Name)
                {
                    Console.WriteLine($"{x.Name} is {_Towns.GetDistance(x)} away.");
                    Console.WriteLine($"Town rating : {x.Rating}{Environment.NewLine}");
                }
            });
            var selection = Console.ReadLine().ToLower();
            var objective = _Towns.TownDefintions.FirstOrDefault(x => x.Name.ToLower() == selection);
            if(objective == null)
            {
                Console.WriteLine($"That selection isn't available..");
                SetTravelContextAndExit(); 
            }

            new TravelEvent(this, Map.towns.First(t=>t.Name == objective.Name));

            return Party.State;
        }

        private PlayerState DisplayBuyMenu()
        {
            Console.WriteLine($"-===========-");
            Console.WriteLine($"{this.Name}'s market. menu");
            Console.WriteLine($"//To purchase items - ex : buy apples 3{Environment.NewLine}");
            Console.WriteLine($"The current items avaialble on the market are :");
            HashSet<ItemType> typeCollection = new HashSet<ItemType>();
            _I.ForEach(x => { typeCollection.Add(x.Type); });
            foreach(var t in typeCollection)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine(Environment.NewLine);
            var itemType = Console.ReadLine().ToLower();
            Console.WriteLine($"Your currency : {Party.State.Currency}. Free capacity : {Party.State.Capacity}{Environment.NewLine}");
            
            var filteredItems = _I.Where(i => i.Rating <= Party.State.Rating && i.Type == Party.State.ParseTypeSelection(itemType)).ToList();
            filteredItems.ForEach(it =>
            {
                Console.WriteLine($"Name : {it.Name}");
                Console.WriteLine($" Price { it.Value * _Towns.PriceMods[it.Type] } Weight { it.Weight}");
                Console.WriteLine($" Stat Modifier : {it.Stat}, {it.StatModifier}");
                Console.WriteLine($"{Environment.NewLine}");
            });

            var selection = Console.ReadLine().Split(" ");
            if(selection[0] == "buy")
            {
                InventoryItem item = filteredItems.FirstOrDefault(x => x.Name.ToLower().Contains(selection[1].ToLower()));
                IEnumerable<InventoryItem> items = filteredItems.Where(x => x.Name.ToLower().Contains(selection[1].ToLower()));

                int amount = 1;
                if (item.Type == ItemType.Goods && !int.TryParse(selection[selection.Length - 1], out amount))
                    amount = 1;

                if(Party.State.Buy(item, this, amount))
                {
                    string totalItemType = item.Type == ItemType.Goods ? Party.State.Inventory.Where(x => x.Name.ToLower().Contains(selection[1].ToLower())).Sum(x => x.Weight) + "lbs" : amount.ToString();
                        
                        
                    Console.WriteLine($"You now have {totalItemType} {item.Name}(s) in your inventory{Environment.NewLine}");
                    Console.WriteLine($"Your currency : {Party.State.Currency}. Free capacity : {Party.State.Capacity} ");
                    Client.SendMap();
                    Client.SendState(Party.Lead.Name + " has bought " + item.Name + " from " + Name + " General Store");
                }

                
                
            }
            return Party.State; 
        }
        private PlayerState DisplaySellMenu()
        {
            Console.WriteLine($"-===========-");
            Console.WriteLine($"{Party.State.Name}'s sellable inventory:");
            Console.WriteLine($"//Type the number of the item you wish to sell");

            int option = 1;
            Console.WriteLine("0. Exit");
            Party.State.Inventory.OrderBy(item => item.Value).ToList().ForEach(item => 
            {
                Console.WriteLine(option + ". " + item.Name + " - $" + item.Value * _Towns.PriceMods[item.Type] + " Weight:" + item.Weight);
                option++;
            });

            int sell = -1;
            while (sell > option || sell < 0)
            {
                Int32.TryParse(Console.ReadLine(), out sell);
            }
            sell -= 1;
            switch(sell)
            {
                case -1:
                    return Party.State;
                    break;
                default:
                    InventoryItem selling = Party.State.Inventory.OrderBy(item => item.Value).ElementAt(sell);
                    Console.WriteLine("Confirm sell (y/n) " + selling.Name + " for $" + selling.Value * _Towns.PriceMods[selling.Type]);
                    if(Console.ReadLine().ToLower() == "y")
                    {
                        Party.State.Sell(selling, this);
                        Client.SendMap();
                        Client.SendState(Party.Lead.Name + " has sold " + selling.Name + " to " + Name + " General Store");
                    }
                    break;
            }

            return Party.State;
        }
    }
}
