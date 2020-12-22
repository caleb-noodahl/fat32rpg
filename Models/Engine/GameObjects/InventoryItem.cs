using GameplayLoopCombat1.classes;
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
        public double Value { get; set; }
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
            Stat = (Stats)_rand.Next(0, 7);
            Weight = _rand.Next(1, 20);
            Value = (double)_rand.Next(0, 300);
            Type = (ItemType)_rand.Next(0, 2);
            switch (Type)
            {
                case ItemType.Armor:
                case ItemType.Weapons:
                    Equip = new Equipment(Type);
                    Name = Equip.Description;
                    StatModifier = Equip.AbilityLevel;
                    break;
                case ItemType.Goods:
                    Name = Names.Goods[_rand.Next(0, Names.Goods.Length)];
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
        Intelligencge
    }
}
