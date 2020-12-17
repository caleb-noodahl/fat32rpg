using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.GameObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Services
{
    public class GameEngineService : IGameEngineService
    {
        private IEventService _eventService; 

        public bool Exit = false;
        private Player _player = new Player();

        
        private Queue<GameEvent> _eventQueue { get; set; } = new Queue<GameEvent>(); 

        public GameEngineService(IEventService eventService,
            IOptions<PlayerSettings> _playerConfig)
        {
            _eventService = eventService;
            _player.ConsumeConfiguration(_playerConfig.Value); 
        }

        public async Task GameLoop()
        {
            //check the current players state
            Console.WriteLine($"{DateTime.Now} Starting GameLoop");
            Console.WriteLine($"Player : {_player.Info.Name} | Currency : {_player.Info.Currency}");
            if (_player.CurrentState.CurrentContext == Context.Unknown)
                _player.UpdatePlayerState(new PlayerState() { NextContext = Context.Welcome });

            while (!this.Exit)
            {


                var events = _eventService.GenerateEvents(_player.CurrentState);

                

                

                for(int i = 0; i < events.Count; i++)
                {
                    var updatedPlayerState = await events[i](_player.CurrentState);
                    _player.UpdatePlayerState(updatedPlayerState);
                }
                
                //determine if any events need to be queued. - 


                var choice = Console.ReadLine(); 
                switch(choice.ToLower())
                {
                    
                }
            }

        }

    }

    public interface IGameEngineService
    {
        Task GameLoop(); 
    }
}
