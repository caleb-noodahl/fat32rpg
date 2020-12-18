using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.Events;
using MerchantRPG.Models.Engine.GameObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Services
{
    public class EventService
    {
        public List<GameEvent> EventManifest { get; set; } = new List<GameEvent>();

        public List<EventAction> GenerateEvents(PlayerState s)
        {
            List<EventAction> response = new List<EventAction>();
            var context = EventManifest.Where(x => x.Context == s.CurrentContext).ToList();

            switch (s.CurrentContext)
            {
                case Context.Town:
                    context = new List<GameEvent>() 
                    { 
                        context.Single(x => x.Name.ToLower() == s.Objective.ToLower() && s.ObjectiveDistance == 0) 
                    };
                    break;
                //we can add other filter logic for events here too like npc fights 
            }
            context.ForEach(x =>
            {
                response.Add(x.EventAction); 
            });
            return response; 
        }
    }

    public interface IEventService 
    {
        public List<EventAction> GenerateEvents(PlayerState s); 
    }
}
