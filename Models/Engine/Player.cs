using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine
{
    public class Player
    {
        private Context _currentContext;

        public Location CurrentLocation { get; set; } = new Location(); 
        public PlayerSettings Info { get; set; } = new PlayerSettings();
        public List<string> Actions { get; set; } = new List<string>(); 
        public PlayerState CurrentState { get
            {
                return new PlayerState()
                {
                    CurrentContext = _currentContext,
                    Rating = this.Info.Rating,
                    Currency = this.Info.Currency,
                    Objective = this.Info.Objective,
                    ObjectiveDistance = this.Info.ObjectiveDistance,
                    Capacity = CalculateCapacity()
                };
            } }

        private int CalculateCapacity()
        {
            int total = 0;
            int modifier = 0;
            this.Inventory.ForEach(x =>
            {
                total = x.Weight;
                if (x.Stat == Stats.Capacity)
                    modifier += x.StatModifier;
            });
            total = total - modifier < 0 ? (total - modifier) * -1 : total - modifier;
            return total; 
        }

        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>(); 

        public Player()
        {

        }

        public void UpdatePlayerState(PlayerState update)
        {
            _currentContext = update.NextContext;
            this.Info.Currency += update.Currency;
            this.Info.Objective = update.Objective;
            this.Info.ObjectiveDistance = update.ObjectiveDistance;
            this.Inventory = update.Inventory;
        }

        internal void ConsumeConfiguration(PlayerSettings value)
        {
            this.Info = value; 
        }

        public async Task HandleGameEvent(GameEvent ev) 
        {
            var state = await ev.EventAction(CurrentState);

        }
    }

}
