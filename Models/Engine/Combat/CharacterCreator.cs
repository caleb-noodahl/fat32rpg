using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Combat
{
    public class CharacterCreator
    {
        public int CharacterPoints = 16;
        public Character result { get; set; }
        public CharacterCreator()
        {
            Console.WriteLine("Please create a character by adding " + CharacterPoints + " points between the following 3 attributes");
            Console.WriteLine("Dexterity - Determines your turn order in combat, likelihood to dodge, and strength of dexterity skills like MultiShot");
            Console.WriteLine("Strength - Determines your maximum health and srength of skills like Cleave");
            Console.WriteLine("Intelligence - Determines the strength of magical abilities, including the ability to heal outside towns");
            Console.WriteLine("You have " + CharacterPoints + " left. How many to Dexterity?");
            int dex = -1;
            while (dex > CharacterPoints || dex < 0)
            {
                Int32.TryParse(Console.ReadLine(), out dex);
            }
            CharacterPoints -= dex;

            Console.WriteLine("You have " + CharacterPoints + " left. How many to Strength?");
            int str = -1;
            while (str > CharacterPoints || str < 0)
            {
                Int32.TryParse(Console.ReadLine(), out str);
            }
            CharacterPoints -= str;

            Console.WriteLine("You have " + CharacterPoints + " left. How many to Intelligence?");
            int intel = -1;
            while (intel > CharacterPoints || intel < 0)
            {
                Int32.TryParse(Console.ReadLine(), out intel);
            }
            CharacterPoints -= intel;

            Console.WriteLine("What is your name?");
            string name = Console.ReadLine();

            Console.WriteLine("Input a difficulty multiplier:");
            Console.WriteLine("0.5 = easy");
            Console.WriteLine("0.75 = normal");
            Console.WriteLine("1 = hard");

            double diff = -1;
            while (diff < 0)
            {
                Double.TryParse(Console.ReadLine(), out diff);
            }
            Party.Difficulty = diff;
            Party.State = new PlayerState();
            Party.State.Name = name;

            result = new Character(name, dex, str, intel);
            result.Player = true;

            Console.WriteLine("Welcome to the world, " + result.Name + ". Over the course of your life you have learned these skills:");
            foreach(KeyValuePair<string, Ability> castable in Abilities.AbilityList.Where(entry => entry.Value.MeetsRequirements(result)))
            {
                Console.WriteLine(castable.Key + " - " + castable.Value.Description);
            }
            Console.WriteLine("---continue---");
            Console.ReadLine();

            Console.WriteLine("Today you're a humble merchant, " + result.Name + ". Tomorrow you might be rich! Or as poor as you are right now.");
            Console.WriteLine("Speaking of which, here's your only possession - a mule. It'll help you carry the many things you're likely to aquire.");
            Party.State.AddToInventory(new InventoryItem()
            {
                Name = "Mule",
                Weight = 0,
                StatModifier = 150,
                Stat = Stats.Capacity
            });
            Console.WriteLine("---continue---");
            Console.ReadLine();


        }
    }
}
