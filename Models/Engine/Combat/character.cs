using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class Character : DSI
    {
        public string Name { get; set; }
        public double HealthMax 
        {
            get
            {
                return Strength * 5;
            }
        }
        public double Health { get; set; }
        public bool Player { get; set; }
        public List<Equipment> Equip = new List<Equipment>();
        public List<StatusEffect> Effects = new List<StatusEffect>();

        public Character(int budget, bool player, bool animal)
        {
            Random rand = new Random();
            int statPick = rand.Next(0, 3);
            Strength = 1 + (Int32)Math.Ceiling(budget * 0.3); //30% to str
            switch (statPick)
            {
                case 0:
                    Dexterity = (Int32)Math.Ceiling(budget * 0.5); //50% to main ability
                    Strength += (Int32)Math.Ceiling(budget * 0.1);
                    Intelligence = (Int32)Math.Ceiling(budget * 0.1);
                    break;
                case 1:
                    Dexterity = (Int32)Math.Ceiling(budget * 0.1);
                    Strength += (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                    Intelligence = (Int32)Math.Ceiling(budget * 0.1);
                    break;
                case 2:
                    Dexterity = (Int32)Math.Ceiling(budget * 0.1);
                    Strength += (Int32)Math.Ceiling(budget * 0.1);
                    Intelligence = (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                    break;

            }

            if(animal)
            {
                int adjChoice = rand.Next(0, Enum.GetValues(typeof(Names.MonsterAdjectives)).Length);
                int aniChoice = rand.Next(0, Names.Animals.Length);
                Name = (Names.MonsterAdjectives)adjChoice + " " + Names.Animals[aniChoice];
            }
            else
            {
                Name = Names.FirstNames[rand.Next(0, Names.FirstNames.Length)];
            }

            Player = player;
            
        }

        public Character(string _name, int _dex, int _str, int _int)
        {
            Name = _name;
            Dexterity = _dex;
            Strength = _str;
            Intelligence = _int;
            Health = HealthMax;
        }
       
        public void AddEquipment(Equipment equipment)
        {
            Dexterity += equipment.Dexterity;
            Strength += equipment.Strength;
            Intelligence += equipment.Intelligence;
            Equip.Add(equipment);
        }

        public void AddEquipmentFromInventoryItem(InventoryItem ii)
        {
            Equip.Add(new Equipment(ii));
        }

        public void RemoveEquipment(Equipment equipment)
        {
            Dexterity -= equipment.Dexterity;
            Strength -= equipment.Strength;
            Intelligence -= equipment.Intelligence;
            Equip.Remove(equipment);
        }

        public double DoDamage(double dmg)
        {
            Random rand = new Random();

            int miss = rand.Next(0, 100);
            double missPercent = Effects.Sum(e => e.MissPercent);
            missPercent += Dexterity;
            if(missPercent > 75)
            {
                missPercent = 75;
            }
            if (miss <= missPercent)
            {
                Console.WriteLine("Miss!");
                return 0;
            }


            Effects.ForEach(effect => 
            {
                dmg *= effect.DamageModifier;
            });
            Health -= dmg;
            if (Health > HealthMax)
                Health = HealthMax;
            return dmg;
        }

        public void TickTurn()
        {
            Effects.ForEach(effect =>
            {
                effect.EffectAction(this);
            });

            Effects.ForEach(effect =>
            {
                if (effect.Turns == 0)
                    Effects.Remove(effect);
                else if (effect.Turns > 0)
                    effect.Turns--;
            });
        }

       
    }
}
