using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class PlayerSettings
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public long Currency { get; set; }
        public double DistanceTraveled { get; set; }
        public string Objective { get; set; }
        public double ObjectiveDistance { get; set; }
    }
}
