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
            StatModifier = _rand.Next(0, 4);
            Stat = (Stats)_rand.Next(0, 3);
            Name = char.ConvertFromUtf32(_rand.Next(1,69));
            Weight = _rand.Next(1, 20);
            Value = (double)_rand.Next(0, 300);
            Type = (ItemType)_rand.Next(0, 2);
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
        Armor
    }
}
