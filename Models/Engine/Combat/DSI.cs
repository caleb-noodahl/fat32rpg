using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class DSI
    {
        public int Dexterity { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }

        public int AbilityLevel {
            get{
                return Dexterity + Strength + Intelligence;
            }
        }

        public string PrimaryStat
        {
            get
            {
                return new Dictionary<string, int>() { { "Dexterity", Dexterity }, { "Strength", Strength }, { "Intelligence", Intelligence } }.OrderByDescending(s => s.Value).First().Key;
            }
        }

        public int PrimaryVal
        {
            get
            {
                return new Dictionary<string, int>() { { "Dexterity", Dexterity }, { "Strength", Strength }, { "Intelligence", Intelligence } }.OrderByDescending(s => s.Value).First().Value;
            }
        }
    }


}
