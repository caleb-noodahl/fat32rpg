using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.GameObjects
{
    public delegate Task<PlayerState> EventAction();
    public abstract class GameEvent
    {
        public Context Context { get; set; }
        public bool IsComplete { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public EventAction EventAction;
    }

}
