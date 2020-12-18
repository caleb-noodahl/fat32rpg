using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public static class Party
    {
        public static List<Character> Members = new List<Character>();
        public static Character Lead { get; set; }
        public static double Difficulty = 0.75;
    }
}
