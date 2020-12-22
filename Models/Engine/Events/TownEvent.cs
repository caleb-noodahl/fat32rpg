using GameplayLoopCombat1.classes;
using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine.GameObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Events
{
    public class TownEvent : GameEvent
    {
        private List<InventoryItem> _i { get; set; } = new List<InventoryItem>();
        public TownSettings _towns; 

        public TownEvent(string name, TownSettings towns, InventorySettings invSettings)
        {
            Random rand = new Random();
            this.Context = Context.Town;
            this.Name = name == string.Empty ? "Rome" : name;
            this.EventAction = Event;
            _i = invSettings.Default;
            _towns = towns;
            _towns.InnName = "The " + Names.Adjective(rand.Next(0, Enum.GetValues(typeof(Names.Adjectives)).Length)) + " " + Names.Things[rand.Next(0, Names.Things.Length)];
        }

        public async Task<PlayerState> Event(PlayerState s)
        {
            Console.WriteLine($"-===========-");
            Console.WriteLine($"Welcome to {this.Name}, " + s.Name);

            while(!IsComplete)
            {
                Console.WriteLine("What is it you'd like to do?");
                Console.WriteLine($"buy or sell or travel or rest or fight{Environment.NewLine}"); 
                switch(Console.ReadLine().ToLower())
                {
                    case "buy": s = DisplayBuyMenu(s);  break;
                    case "sell": s = DisplayBuyMenu(s); break;
                    case "travel": s = SetTravelContextAndExit(s); IsComplete = true; break;
                    case "rest": s = Rest(s); break;
                    case "fight": new GameplayLoopCombat1.classes.Combat(Party.Members); break;
                    default: Console.WriteLine($"I'm afraid that isn't an option...{Environment.NewLine}"); break;
                }
            }
            return s;
        }

        private PlayerState Rest(PlayerState s)
        {
            int price = new Random().Next(1, 5) * 10;
            Console.WriteLine("Welcome to " + _towns.InnName + ", a night here costs " + price + " gold. You staying? (y/n)");
            switch(Console.ReadLine())
            {
                case "y":
                    if (s.Spend(price))
                    {
                        Party.Members.ForEach(member =>
                        {
                            member.DoDamage(-10);
                        });
                        Console.WriteLine("Party healed for 10 at the local inn");
                    }
                    else
                        Console.WriteLine("You can't afford " + price + "? Get a job you bumb!");
                    break;
                default:
                    break;
            }
            return s;

        }

        private PlayerState SetTravelContextAndExit(PlayerState s)
        {
            Console.WriteLine($"{Environment.NewLine}Which city would you like to embark for?");
            _towns.TownDefintions.ForEach(x => 
            {
                if(x.Name != this.Name)
                {
                    Console.WriteLine($"{x.Name} is {_towns.GetDistance(x)} away.");
                    Console.WriteLine($"Town rating : {x.Rating}{Environment.NewLine}");
                }
            });
            var selection = Console.ReadLine().ToLower();
            var objective = _towns.TownDefintions.FirstOrDefault(x => x.Name.ToLower() == selection);
            if(objective == null)
            {
                Console.WriteLine($"That selection isn't available..");
                SetTravelContextAndExit(s); 
            }
            s.Currency = 0;
            s.NextContext = Context.Travel;
            s.Objective = selection.ToLower();
            s.ObjectiveDistance = _towns.GetDistance(objective);
            return s;
        }

        private PlayerState DisplayBuyMenu(PlayerState s)
        {
            Console.WriteLine($"-===========-");
            Console.WriteLine($"{this.Name}'s market. menu");
            Console.WriteLine($"//To purchase items - ex : buy apples 3{Environment.NewLine}");
            Console.WriteLine($"The current items avaialble on the market are :");
            HashSet<ItemType> typeCollection = new HashSet<ItemType>();
            _i.ForEach(x => { typeCollection.Add(x.Type); });
            foreach(var t in typeCollection)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine(Environment.NewLine);
            var itemType = Console.ReadLine().ToLower();
            Console.WriteLine($"Your currency : {s.Currency}. Free capacity : {s.Capacity}{Environment.NewLine}");
            
            var filteredItems = _i.Where(i => i.Rating <= s.Rating && i.Type == s.ParseTypeSelection(itemType)).ToList();
            filteredItems.ForEach(it =>
            {
                Console.WriteLine($"Name : {it.Name}");
                Console.WriteLine($" Price { it.Value} Weight { it.Weight}");
                Console.WriteLine($" Stat Modifier : {it.Stat}, {it.StatModifier}");
                Console.WriteLine($"{Environment.NewLine}");
            });

            var selection = Console.ReadLine().Split(" ");
            if(selection[0] == "buy")
            {
                var item = filteredItems.FirstOrDefault(x => x.Name.ToLower().Contains(selection[1].ToLower()));
                if(item != null)
                {
                    int amount = 0;
                    if (!int.TryParse(selection[selection.Length - 1], out amount))
                        amount = 1;
                    
                    //check to make sure the player can afford the transaction
                    if(item.Value * amount > s.Currency)
                    {
                        Console.WriteLine($"It doesn't look like you can afford that");
                        DisplayBuyMenu(s);
                        return s;
                    }
                    //check to make sure the player can 
                    if(item.Weight * amount > s.Capacity)
                    {
                        Console.WriteLine($"You wouldn't be able carry all of it");
                        DisplayBuyMenu(s);
                        return s;
                    }
                    for(int i = 0; i < amount; i++)
                    {
                        s.Inventory.Add(item);
                        s.Capacity -= item.Weight;
                    }
                        
                    s.Currency += (long)item.Value * amount * -1; 
                    var totalItemType = s.Inventory.Where(x => x.Name.ToLower().Contains(selection[1].ToLower())).Count();
                    item.Equip.DistributeEquipment(Party.Lead, true);
                    Console.WriteLine($"You now have {totalItemType} {item.Name}(s) in your inventory{Environment.NewLine}");
                    Console.WriteLine($"Your currency : {s.Currency}. Free capacity : {s.Capacity} ");
                }
            }
            return s; 
        }
    }
}
