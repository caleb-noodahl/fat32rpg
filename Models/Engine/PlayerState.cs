using System;
using System.Collections.Generic;
using System.Text;
using GameplayLoopCombat1.classes;
using MerchantRPG.Models.Engine.GameObjects;

namespace MerchantRPG.Models.Engine
{
    public class PlayerState
    {
        public Context CurrentContext { get; set; }
        public Context NextContext { get; set; }
        public double Rating { get; set; }
        public long Currency { get; set; } = 1000;
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public string Objective { get; set; }
        public double ObjectiveDistance { get; set; }
        public int Capacity { get; set; } = 100;
        public int MaxCapacity { get; set; } = 100;
        public string Name { get; set; }

        public PlayerState()
        {
            Currency = (long)Math.Floor((1.5 - Party.Difficulty) * Currency);
            MaxCapacity = (int)Math.Floor((1.5 - Party.Difficulty) * Capacity);
            Capacity = MaxCapacity;
        }

        public PlayerState(string name, int _dex, int _str, int _int)
        {
            this.Name = name;
            CurrentContext = Context.Unknown;
            NextContext = Context.Unknown;
            Rating = 0;
            Objective = string.Empty;
            ObjectiveDistance = 0;
            Capacity = 0;
        }

        internal ItemType ParseTypeSelection(string ts)
        {
            ItemType res = ItemType.Goods;
            switch (ts) 
            {
                case "weapons": res = ItemType.Weapons; break;
                case "armor": res = ItemType.Armor; break;
            }
            return res;

        }

        public bool Spend(long amt)
        {
            if(Currency >= amt)
            {
                Currency -= amt;
                return true;
            }
            return false;
        }

    }

}
