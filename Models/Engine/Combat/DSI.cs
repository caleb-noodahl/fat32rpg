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
        public int DexXP { get; set; } = 0;
        public int StrXP { get; set; } = 0;
        public int IntXP { get; set; } = 0;

        public int AbilityLevel 
        {
            get
            {
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

        public void AddXP(Requirement req)
        {
            DexXP += req.Dexterity - (Dexterity / 5);
            StrXP += req.Strength - (Strength / 5);
            IntXP += req.Intelligence - (Intelligence / 5);
            
            if(DexXP >= 100)
            {
                DexXP -= 100;
                Dexterity++;
                Console.WriteLine("Dexterity has leveled up to " + Dexterity);
            }
            if (StrXP >= 100)
            {
                StrXP -= 100;
                Strength++;
                Console.WriteLine("Strength has leveled up to " + Strength);
            }
            if (IntXP >= 100)
            {
                IntXP -= 100;
                Intelligence++;
                Console.WriteLine("Intelligence has leveled up to " + Intelligence);
            }
        }
    }


}
