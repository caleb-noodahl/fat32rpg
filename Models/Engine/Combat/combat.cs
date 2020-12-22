using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class Combat
    {
        public Character[] Participants { get; set; }
        public int Turn = 0;
        public bool Flee = false;
        public bool GameOver = false;
        public Random rand = new Random();

        public Combat(List<Character> _participants, bool generateEnemies = true)
        {
            Participants = _participants.OrderByDescending(c => c.Dexterity).ToArray();
            if (generateEnemies)
                GenerateEnemies();

            Console.WriteLine("Combat!");
            Console.WriteLine("Combat Participants:");
            foreach(Character person in Participants)
            {
                Console.WriteLine(person.Name + " - Lvl:" + person.AbilityLevel + " HP: " + person.Health);
            }
            Console.WriteLine("---continue---");
            Console.ReadLine();

            while (true)
            {
                if (Flee)
                {
                    Console.WriteLine("You flee!");
                    ClearStatuses();
                    return;
                }

                if (GameOver)
                {
                    Party.State.Capacity = Party.State.MaxCapacity;
                    Party.State.Inventory.Clear();
                    Party.Members.Clear();
                    Party.Members.Add(Party.Lead);

                    Console.WriteLine("Your entire party dead, the " + Participants.Where(p => p.Player = false && p.Health > 0).First().Name + " seems thoroughly delighted, as if having won a game. You lay face down in the dirt, sure you'll die this time.");
                    int lossOutcome = rand.Next(0, 100);
                    if (lossOutcome <= 5) {
                        Party.State.Currency = 0;
                        Console.WriteLine("There was no glory in your final moments. You lay on the roadside waiting for the travelers, theives, anything. You die bleeding out, your corpse only found and looted once the stench attracted passersby. Your seemingly lifeless corpse is dragged back to town for burial when you awake!");
                        Party.Lead.Health = 1;
                    }
                    else if(lossOutcome <= 20)
                    {
                        Party.State.Currency = 0;
                        Console.WriteLine("As you feel yourself slipping away you hear voices. You moan, but they go straight for your caravan. Once of the voices stands above you, you feel them rifle through your pockets and take everything you own.");
                        Console.WriteLine("You don't know how long later, but you wake up in a trailer on a makeshift hay bed. A grumpy old man with a disaproving look on his face sees you move.");
                        Console.WriteLine("'You're lucky I found you. Don't worry, we'll be in town soon.'");
                        Party.Members.Add(new Character(Party.Lead.AbilityLevel-5, true, false));
                    }
                    else
                    {
                        Party.State.Currency /= (lossOutcome / 10);
                        Console.WriteLine("Broken and beaten, you manage to crawl off with " + Party.State.Currency + " gold but none of your gear. You limp back to town.");
                    }


                    ClearStatuses();
                    return;
                }

                if(Participants.Where(p => p.Player == false && p.Health > 0).Count() == 0)
                {
                    Console.WriteLine("VICTORY!");
                    ClearStatuses();
                    if (FindDrops.Search(Participants))
                    {
                        foreach(Character enemy in Participants.Where(p => p.Health <= 0 && !p.Player))
                        {
                            new Equipment(enemy);
                        }
                    }
                    return;
                }
                    
                NewTurn();
            }
            
        }

        public void ClearStatuses()
        {
            foreach(Character c in Participants)
            {
                c.Effects.Clear();
            }

            foreach(KeyValuePair<string, Ability> kvp in Abilities.AbilityList)
            {
                kvp.Value.LastCast.Clear();
            }
        }

        public void NewTurn()
        {
            Turn++;
            for (int subturn = 0; subturn < Participants.Length; subturn++)
            {
                if(Participants[subturn].Health > 0)
                {
                    Console.WriteLine("Turn " + Turn + "." + subturn + " - " + Participants[subturn].Name);
                    IEnumerable<KeyValuePair<string, Ability>> castable = Abilities.AbilityList.Where(entry => entry.Value.MeetsRequirements(Participants[subturn], Turn));
                    if (Participants[subturn].Player)
                        PlayerTurn(Participants[subturn], castable);
                    else
                    {
                        int npcChoice = rand.Next(0, castable.Count());
                        KeyValuePair<string, Ability>[] castableArr = castable.ToArray();
                        List<Character> playerTargets = Participants.Where(chars => chars.Player == true).ToList();
                        List<Character> friendlyTargets = Participants.Where(chars => chars.Player == false).ToList();
                        if (castableArr[npcChoice].Key.Contains("Heal"))
                        {
                            playerTargets = friendlyTargets;
                        }

                        AbilityResponse actionResult = castableArr[npcChoice].Value.Action(Participants[subturn], playerTargets.ToArray(), Turn, castableArr[npcChoice].Key);
                        Console.WriteLine(Participants[subturn].Name + " uses " + castableArr[npcChoice].Key);
                        Console.WriteLine(actionResult.Message);
                    }

                    if (Flee)
                        return;
                    if(Participants.Where(p => p.Player && p.Health > 0).Count() == 0)
                    {
                        GameOver = true;
                        return;
                    }
                    Participants[subturn].TickTurn();
                    Console.WriteLine("---continue---");
                    Console.ReadLine();
                }
            }
            
        }

        public void PlayerTurn(Character player, IEnumerable<KeyValuePair<string, Ability>> castable)
        {
            KeyValuePair<string, Ability>[] castableArr = castable.ToArray();
            int option = 0;
            Console.WriteLine("Your Turn! Type the number of your selection");
            Console.WriteLine(option + ". Flee!");
            foreach(KeyValuePair<string, Ability> ability in castableArr)
            {
                option++;
                Console.WriteLine(option + "." + ability.Key + " - " + ability.Value.Description);
            }

            int choice = -1;
            while(choice > option || choice < 0) 
            {
                Int32.TryParse(Console.ReadLine(), out choice);
            }

            if(choice == 0)
            {
                Flee = true;
                return;
            }

            Console.WriteLine("Who would you like to target with " + castableArr[choice - 1].Key + "?");
            Console.WriteLine("0. Finished selecting targets");
            int targetOption = 1;
            foreach (Character character in Participants)
            {
                
                Console.WriteLine(targetOption + "." + character.Name + " - Health: " + character.Health);
                targetOption++;
            }

            int targetChoice = -1;
            List<Character> targets = new List<Character>();
            while (targetChoice != 0 && (targetChoice > targetOption || targetChoice < 0 || targets.Count() < castableArr[choice - 1].Value.MaxTargets))
            {
                Int32.TryParse(Console.ReadLine(), out targetChoice);
                if(targetChoice < targetOption && targetChoice > 0 && !targets.Exists(e => e == Participants[targetChoice - 1]))
                {
                    targets.Add(Participants[targetChoice - 1]);
                    Console.WriteLine("Added target " + Participants[targetChoice - 1].Name);
                }
            }

            AbilityResponse actionResult = castableArr[choice - 1].Value.Action(player, targets.ToArray(), Turn, castableArr[choice - 1].Key);
            if (!Abilities.AbilityList[castableArr[choice - 1].Key].LastCast.ContainsKey(player))
                Abilities.AbilityList[castableArr[choice - 1].Key].LastCast.Add(player, Turn);
            else
                Abilities.AbilityList[castableArr[choice - 1].Key].LastCast[player] = Turn;
            Console.WriteLine(actionResult.Message);
        }

        public void GenerateEnemies()
        {
            IEnumerable<Character> players = Participants.Where(p => p.Player);
            int ability = players.Sum(p => p.AbilityLevel);
            int enemyAbility = (Int32)Math.Ceiling(Party.Difficulty* ability);
            int enemyCount = rand.Next(1, players.Count()+6);

            List<Character> participantsL = Participants.ToList();

            for(int n = 0; enemyCount > n; n++)
            {
                int budget = enemyAbility / enemyCount;
                int statPick = rand.Next(0, 3);
                int str = 1 + (Int32)Math.Ceiling(budget * 0.3); //30% to str
                int dex = 0;
                int intel = 0;
                switch(statPick)
                {
                    case 0:
                        dex = (Int32)Math.Ceiling(budget * 0.5); //50% to main ability
                        str += (Int32)Math.Ceiling(budget * 0.1);
                        intel = (Int32)Math.Ceiling(budget * 0.1);
                        break;
                    case 1:
                        dex = (Int32)Math.Ceiling(budget * 0.1); 
                        str += (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                        intel = (Int32)Math.Ceiling(budget * 0.1);
                        break;
                    case 2:
                        dex = (Int32)Math.Ceiling(budget * 0.1);
                        str += (Int32)Math.Ceiling(budget * 0.1);
                        intel = (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                        break;

                }

                int adjChoice = rand.Next(0, Enum.GetValues(typeof(Names.MonsterAdjectives)).Length);
                int aniChoice = rand.Next(0, Names.Animals.Length);

                participantsL.Add(new Character((Names.MonsterAdjectives)adjChoice + " " + Names.Animals[aniChoice], dex, str, intel));
                
            }

            Participants = participantsL.OrderByDescending(c => c.Dexterity).ToArray();

        }

        

        
    }
}
