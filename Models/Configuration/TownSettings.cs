using MerchantRPG.Models.Engine.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class TownSettings
    {
        public List<TownDefintion> TownDefintions { get; set; } = new List<TownDefintion>(); 
        public TownSettings() { }
        public TownSettings(List<TownDefintion> townDefs)
        {
            TownDefintions = townDefs;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string InnName { get; set; }


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
