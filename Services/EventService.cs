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
    public class EventService : IEventService
    {
        private GameEventSettings _eventSettings;
        private List<GameEvent> _eventManifest { get; set; } = new List<GameEvent>();
        private InventorySettings _invSettings { get; set; } = new InventorySettings();
        private TownSettings _towns { get; set; } = new TownSettings(); 

        public EventService(IOptions<GameEventSettings> eventSettings, IOptions<TownSettings> towns, 
            IOptions<InventorySettings> invSettings)
        {
            _eventSettings = eventSettings.Value;
            _invSettings = invSettings.Value;
            _towns = towns.Value;
            _eventManifest = GenerateEventManifest();
        }

        private List<GameEvent> GenerateEventManifest()
        {
            List<GameEvent> response = new List<GameEvent>();
            response.Add(new WelcomeEvent());
            response.Add(new TownEvent("", _towns, _invSettings));
            response.Add(new TownEvent("neapolis", _towns, _invSettings));

            //default events loaded from configuration
            return response; 
        }

        public List<EventAction> GenerateEvents(PlayerState s)
        {
            List<EventAction> response = new List<EventAction>();
            var context = _eventManifest.Where(x => x.Context == s.CurrentContext).ToList();

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
