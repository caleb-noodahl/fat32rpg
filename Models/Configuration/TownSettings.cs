using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Engine.Events;
using MerchantRPG.Models.Engine.GameObjects;
using Newtonsoft.Json;
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
            PriceMods.Add(ItemType.Armor, (double)rand.Next(20, 200) / 100);
            PriceMods.Add(ItemType.Weapons, (double)rand.Next(20, 200) / 100);
            PriceMods.Add(ItemType.Goods, (double)rand.Next(20, 200) / 100);
            int numMercs = rand.Next(1, 4);
            for (int i = 0; i < numMercs; i++)
            {
                Mercs.Add(new Character(Party.Lead.AbilityLevel * rand.Next(20, 200) / 100, true, false));
            }
        }

        [JsonConstructor]
        public TownSettings(List<TownDefintion> townDefinitions, int x, int y, string innName, List<Character> mercs, Dictionary<ItemType, double> priceMods)
        {
            TownDefintions = townDefinitions;
            X = x;
            Y = y;
            InnName = innName;
            Mercs = mercs;
            PriceMods = priceMods;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string InnName { get; set; }
        public List<Character> Mercs { get; set; } = new List<Character>();
        public Dictionary<ItemType, double> PriceMods { get; set; } = new Dictionary<ItemType, double>() {
        };


        public double GetDistance(TownEvent town)
        {
            return Math.Sqrt(Math.Pow(X - town._Towns.X, 2) + Math.Pow(Y - town._Towns.Y, 2));
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

        public TownDefintion()
        {

        }

        [JsonConstructor]
        public TownDefintion(string name, int rating, double distance, int x, int y)
        {
            Name = name;
            Rating = rating;
            Distance = distance;
            X = x;
            Y = y;
        }

    }
}
