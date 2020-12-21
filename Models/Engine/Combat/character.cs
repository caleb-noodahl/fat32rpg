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

        public Character()
        {

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
