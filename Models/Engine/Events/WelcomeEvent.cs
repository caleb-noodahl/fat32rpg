using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Events
{
    public class WelcomeEvent : GameEvent
    {
        public WelcomeEvent()
        {
            this.Context = Context.Welcome;
            this.EventAction = Event;
        }

        public async Task<PlayerState> Event()
        {
            Console.WriteLine("Greetings! Today you're a humble merchant. Tomorrow you might be rich! Or as poor as you are right now.");
            Console.WriteLine("Speaking of which, lets get you setup.");
             Party.State.Currency = 100;
             Party.State.Inventory.Add(new InventoryItem()
            {
                Name = "Mule",
                Weight = 100,
                StatModifier = 150,
                Stat = Stats.Capacity
            });

             Party.State.NextContext = Context.Town;
             Party.State.CurrentContext = Context.Unknown;
             Party.State.Objective = "Rome";
             Party.State.ObjectiveDistance = 0;

            Console.WriteLine(string.Empty);
            Console.WriteLine("There we go. You've got a bit of cash in your pocket, and a mule to cart around your goods.");
            Console.WriteLine($"Current Currency : { Party.State.Currency}");
             Party.State.Inventory.ForEach(x => Console.WriteLine($"Inventory Item : {x.Name} | {x.Stat.ToString()} | {x.StatModifier - x.Weight} | "));
            Console.WriteLine($"{Environment.NewLine}Hit Enter"); 

            return Party.State;
        }

    }
}
