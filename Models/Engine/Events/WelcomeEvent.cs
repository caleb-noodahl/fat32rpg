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

        public async Task<PlayerState> Event(PlayerState p)
        {
            Console.WriteLine("Greetings! Today you're a humble merchant. Tomorrow you might be rich! Or as poor as you are right now.");
            Console.WriteLine("Speaking of which, lets get you setup.");
            p.Currency = 100;
            p.Inventory.Add(new InventoryItem()
            {
                Name = "Mule",
                Weight = 100,
                StatModifier = 150,
                Stat = Stats.Capacity
            });

            p.NextContext = Context.Town;
            p.CurrentContext = Context.Unknown;
            p.Objective = "Rome";
            p.ObjectiveDistance = 0;

            Console.WriteLine(string.Empty);
            Console.WriteLine("There we go. You've got a bit of cash in your pocket, and a mule to cart around your goods.");
            Console.WriteLine($"Current Currency : {p.Currency}");
            p.Inventory.ForEach(x => Console.WriteLine($"Inventory Item : {x.Name} | {x.Stat.ToString()} | {x.StatModifier - x.Weight} | "));
            Console.WriteLine($"{Environment.NewLine}Hit Enter"); 

            return p;
        }

    }
}
