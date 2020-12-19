using MerchantRPG.Models.Engine;
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
    }
}
