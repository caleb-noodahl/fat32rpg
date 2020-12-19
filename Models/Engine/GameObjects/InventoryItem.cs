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
            StatModifier = _rand.Next(0, 5);
            Stat = (Stats)_rand.Next(0, 7);
            Weight = _rand.Next(1, 20);
            Value = (double)_rand.Next(0, 300);
            Type = (ItemType)_rand.Next(0, 2);
            switch (Type)
            {
                case ItemType.Armor:
                    Name = Goods[_rand.Next(0, Goods.Length)];
                    break;
                case ItemType.Weapons:
                    Name = char.ConvertFromUtf32(_rand.Next(1, 69));
                    break;
                case ItemType.Goods:
                    Name = char.ConvertFromUtf32(_rand.Next(1, 69));
                    break;
            }
        }

        public string[] Goods =
        {
            "Glasses", "Apples", "Seeds", "Corn", "Hay", "Beaver Pelts", "Salt", "Cocaine", "Flour", "Wheat", "Rye", "Lumber", "Iron Ore", "Copper Ore", "Bronze Bars", "Tin Ore", "Eggs", "Rope"
        };



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
