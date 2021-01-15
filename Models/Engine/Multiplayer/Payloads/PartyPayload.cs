using MerchantRPG.Models.Engine.Combat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MerchantRPG.Models.Engine.Multiplayer.Payloads
{
    public class PartyPayload
    {
        public PlayerState State { get; set; }
        public List<Character> Members { get; set; }
        public Guid MPID { get; set; }

        [JsonConstructor]
        public PartyPayload(PlayerState state, List<Character> members, Guid mpid)
        {
            MPID = mpid;
            State = state;
            Members = members;
        }
    }
}
