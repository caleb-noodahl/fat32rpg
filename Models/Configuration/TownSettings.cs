using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class TownSettings
    {
        public List<TownDefintion> TownDefintions { get; set; } = new List<TownDefintion>(); 
    }

    public class TownDefintion
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public double Distance { get; set; }
    }
}
