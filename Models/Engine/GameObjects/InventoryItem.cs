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
        public List<string> Actions { get; set; } = new List<string>();
        public int Rating { get; set; }
        public ItemType Type { get; set; }

        public double ValueWithMercantileModifier(int modifier)
        {
            return Value;
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
