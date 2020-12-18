using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class GameEventSettings
    {
        public List<GameEventDefinition> EventDefintions { get; set; } = new List<GameEventDefinition>(); 
        public GameEventSettings() { }
    }

    public class GameEventDefinition
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public Context Context { get; set; }

    }
}
