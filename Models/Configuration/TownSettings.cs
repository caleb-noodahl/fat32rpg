using MerchantRPG.Models.Engine.Events;
using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class TownSettings
    {
        public List<TownDefintion> TownDefintions { get; set; } = new List<TownDefintion>(); 
        public TownSettings(List<TownDefintion> townDefs)
        {
            Random rand = new Random();
            TownDefintions = townDefs;
            priceMods.Add(ItemType.Armor, (double)rand.Next(20, 200) / 100);
            priceMods.Add(ItemType.Weapons, (double)rand.Next(20, 200) / 100);
            priceMods.Add(ItemType.Goods, (double)rand.Next(20, 200) / 100);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string InnName { get; set; }
        public Dictionary<ItemType, double> priceMods { get; set; } = new Dictionary<ItemType, double>() {
        };


        public double GetDistance(TownEvent town)
        {
            return Math.Sqrt(Math.Pow(X - town._towns.X, 2) + Math.Pow(Y - town._towns.Y, 2));
        }

        public double GetDistance(TownDefintion town)
        {
            return Math.Sqrt(Math.Pow(X - town.X, 2) + Math.Pow(Y - town.Y, 2));
        }
    }

    public class TownDefintion
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public double Distance { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
}
