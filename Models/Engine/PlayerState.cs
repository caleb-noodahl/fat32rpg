using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Engine.Events;
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
        public int Capacity 
        { 
            get
            {
                return MaxCapacity - Inventory.Sum(i => i.Weight);
            }
        }
        private int _MaxCapacity = 150;
        public int MaxCapacity 
        {
            get
            {
                return _MaxCapacity + Inventory.Where(i => i.Stat == Stats.Capacity).Sum(i => i.StatModifier);
            }
            set
            {
                _MaxCapacity = value;
            }
        }
        public string Name { get; set; }

        public PlayerState()
        {
            Currency = (long)Math.Floor((1.5 - Party.Difficulty) * Currency);
            if (Currency < 100)
                Currency = 100;
            MaxCapacity = (int)Math.Floor((1.5 - Party.Difficulty) * Capacity);
            if (MaxCapacity < 25)
                MaxCapacity = 25;
        }

        public PlayerState(string name, int _dex, int _str, int _int)
        {
            this.Name = name;
            CurrentContext = Context.Unknown;
            NextContext = Context.Unknown;
            Rating = 0;
            Objective = string.Empty;
            ObjectiveDistance = 0;
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

        public bool Buy(InventoryItem buying, TownEvent buyingFrom, int amount = 1)
        {
            int weightForGoods = 0;
            long valueForGoods = 0;
            long cost;


            if (buying.Type == ItemType.Goods)
            {
                valueForGoods = (long)(buying.Value / (double)buying.Weight * (double)amount * buyingFrom._towns.priceMods[buying.Type]);
                weightForGoods = amount;
                cost = valueForGoods;
                
                InventoryItem storeItem = buyingFrom._i.First(i => i.Name == buying.Name);
                if (amount > storeItem.Weight)
                    return false;

            }
            else
                cost = (long)(buying.Value * buyingFrom._towns.priceMods[buying.Type]);

            if (Spend(cost))
            {
                if (buying.Type == ItemType.Goods && Inventory.Exists(i => i.Name == buying.Name))
                {
                    InventoryItem item = Inventory.First(i => i.Name == buying.Name);
                    item.Weight += weightForGoods;
                    item.Value += valueForGoods;
                    InventoryItem storeItem = buyingFrom._i.First(i => i.Name == buying.Name);
                    storeItem.Value -= valueForGoods;
                    storeItem.Weight -= weightForGoods;
                    if(storeItem.Weight <= 0)
                        buyingFrom._i.Remove(buying);
                }
                else if(buying.Type == ItemType.Goods)
                {
                    Inventory.Add(new InventoryItem()
                    {
                        Name = buying.Name,
                        Weight = weightForGoods,
                        Value = valueForGoods,
                        Type = ItemType.Goods,
                        Stat = buying.Stat,
                        StatModifier = buying.StatModifier,
                        Rating = buying.Rating
                    });
                    InventoryItem storeItem = buyingFrom._i.First(i => i.Name == buying.Name);
                    storeItem.Value -= valueForGoods;
                    storeItem.Weight -= weightForGoods;
                    if (storeItem.Weight <= 0)
                        buyingFrom._i.Remove(buying);
                }
                else if (AddToInventory(buying))
                {
                    if (buying.Equip != null)
                        buying.Equip.DistributeEquipment(Party.Lead, true);
                    buyingFrom._i.Remove(buying);
                    return true;
                }
                else
                {
                    Spend(cost * -1);
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("Not enough funds to buy " + buying.Name + " for $" + cost + ". Your funds: " + Currency);
                return false;
            }
        }


        public bool Sell(InventoryItem selling, TownEvent sellingTo)
        {
            if (!Inventory.Contains(selling))
            {
                Console.WriteLine(selling.Name + " is not in your inventory");
                return false;
            }

            Inventory.Remove(selling);

            if(selling.Equip != null)
            {
                Party.Members.Find(c => c.Equip.Contains(selling.Equip)).Equip.Remove(selling.Equip);
            }

            sellingTo._i.Add(selling);

            double soldVal = selling.Value * sellingTo._towns.priceMods[selling.Type];
            Currency += (long)soldVal;

            Console.WriteLine("Successfully sold " + selling.Name + " to the shop at " + sellingTo.Name + " for " + soldVal);
            return true;
        }

        public bool AddToInventory(InventoryItem item)
        {
            if (Capacity < item.Weight)
            {
                Console.WriteLine("Not enough room to carry " + item.Name + " (" + item.Weight + " weight)");
                return false;
            }

            Inventory.Add(item);
            Console.WriteLine("You've added a " + item.Name + " to your inventory");
            return true;
        }

    }

}
