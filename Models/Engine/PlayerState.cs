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
        public long Currency { get; set; }
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public string Objective { get; set; }
        public double ObjectiveDistance { get; set; }
        public int Capacity { get; set; }

        public PlayerState() { }

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
