using System;
using System.Collections.Generic;
using System.Text;
using GameplayLoopCombat1.classes;
using MerchantRPG.Models.Engine.GameObjects;

namespace MerchantRPG.Models.Engine
{
    public class PlayerState : Character
    {
        public Context CurrentContext { get; set; }
        public Context NextContext { get; set; }
        public double Rating { get; set; }
        public long Currency { get; set; } = 1000;
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public string Objective { get; set; }
        public double ObjectiveDistance { get; set; }
        public int Capacity { get; set; } = 100;

        public PlayerState() 
        {
            Currency = (long)Party.Difficulty * Currency;
            Capacity = (int)Math.Floor(Party.Difficulty * Capacity);
        }

        public PlayerState(string name, int _dex, int _str, int _int) : base(name, _dex, _str,_int)
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

    }

}
