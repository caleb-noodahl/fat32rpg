using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MerchantRPG.Models.Engine.Multiplayer.Payloads
{
    public class MapPayload
    {
        public List<TownEvent> Towns { get; set; }
        public Guid MPID { get; set; }

        [JsonConstructor]
        public MapPayload(List<TownEvent> towns, Guid mpid)
        {
            Towns = towns;
            MPID = mpid;
        }
    }
}
