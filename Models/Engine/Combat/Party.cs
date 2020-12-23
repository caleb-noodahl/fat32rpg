using MerchantRPG.Models.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Combat
{
    public static class Party
    {
        public static List<Character> Members = new List<Character>();
        public static Character Lead { get; set; }
        public static PlayerState State { get; set; }
        public static double Difficulty { get; set; } = 0.75;
    }
}
