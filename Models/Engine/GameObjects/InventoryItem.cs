using MerchantRPG.Models.Engine.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine.GameObjects
{
    public class InventoryItem
    {
        public int StatModifier { get; set; }
        public Stats Stat { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public long Value { get; set; }
        public int Rating { get; set; }
        public ItemType Type { get; set; }
        public Equipment Equip { get; set; }

        public double ValueWithMercantileModifier(int modifier)
        {
            return Value;
        }

        public InventoryItem()
        {

        }

        public InventoryItem(bool rand)
        {
            var _rand = new Random();
            StatModifier = 0;
            Stat = Stats.None;
            
            Value = _rand.Next(0, 300);
            Type = (ItemType)_rand.Next(0, 3);
            switch (Type)
            {
                case ItemType.Armor:
                case ItemType.Weapons:
                    Equip = new Equipment(Type);
                    Name = Equip.Description;
                    StatModifier = Equip.AbilityLevel;
                    Enum.TryParse(Equip.PrimaryStat, out Stats _Stat); ;
                    Stat = _Stat;
                    Value = Equip.AbilityLevel * _rand.Next(50, 100);
                    Weight = (Math.Abs(Equip.AbilityLevel) + 1) * _rand.Next(2, 10);
                    break;
                case ItemType.Goods:
                    Name = Names.Goods[_rand.Next(0, Names.Goods.Length)];
                    Weight = _rand.Next(10, 1000);
                    break;
            }
        }


    }
    public enum ItemType
    {
        Goods,
        Weapons,
        Armor
    }
    public enum Stats
    {
        None,
        Capacity,
        Speech,
        Mercantile,
        Armor,
        Dexterity,
        Strength,
        Intelligence
    }
}
