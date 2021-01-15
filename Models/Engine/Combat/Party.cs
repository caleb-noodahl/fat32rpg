using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.Multiplayer.Payloads;
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
        public static bool Multiplayer { get; set; } = false;
        public static Guid MPID { get; set; } = new Guid();
        public static List<PartyPayload> MultiMembers { get; set; } = new List<PartyPayload>();
        public static bool Host { get; set; } = false;
        public static PartyPayload MPPayload
        {
            get
            {
                return new PartyPayload(State, Members, MPID);
            }
        }
    }
}
