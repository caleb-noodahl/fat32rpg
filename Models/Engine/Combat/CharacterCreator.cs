﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class CharacterCreator : DSI
    {
        public int CharacterPoints = 12;
        public Character result { get; set; }
        public CharacterCreator()
        {
            Console.WriteLine("Please create a character by adding " + CharacterPoints + " points between the following 3 attributes");
            Console.WriteLine("Dexterity - Determines your turn order in combat and strength of dexterity skills like MultiShot");
            Console.WriteLine("Strength - Determines your maximum health and srength of skills like Cleave");
            Console.WriteLine("Intelligence - Determines the strength of magical abilities");
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

            result = new Character(name, dex, str, intel);
            result.Player = true;
        }
    }
}