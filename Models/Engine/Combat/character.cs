using MerchantRPG.Models.Engine;
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
        public double HealthMax { get; set; }
        public double Health { get; set; }
        public bool Player { get; set; }
        public List<Equipment> Equip = new List<Equipment>();

        public Character()
        {

        }

        public Character(string _name, int _dex, int _str, int _int)
        {
            Name = _name;
            Dexterity = _dex;
            Strength = _str;
            Intelligence = _int;
            HealthMax = Strength * 5;
            Health = HealthMax;
        }
       
        public void AddEquipment(Equipment equipment)
        {
            Dexterity += equipment.Dexterity;
            Strength += equipment.Strength;
            Intelligence += equipment.Intelligence;
            Equip.Add(equipment);
        }

        public void RemoveEquipment(Equipment equipment)
        {
            Dexterity -= equipment.Dexterity;
            Strength -= equipment.Strength;
            Intelligence -= equipment.Intelligence;
            Equip.Remove(equipment);
        }
    }
}
