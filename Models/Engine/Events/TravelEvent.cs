using GameplayLoopCombat1.classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine.Events
{
    public class TravelEvent
    {
        public TownEvent Origin { get; set; }
        public TownEvent Destination { get; set; }
        public int Forward { get; set; } = 1;
        public int Progress { get; set; } = 1;
        public double Distance 
        { 
            get
            {
                return Origin._towns.GetDistance(Destination);
            }
        }

        public TravelEvent(TownEvent _origin, TownEvent _destination)
        {
            Origin = _origin;
            Destination = _destination;
            Console.WriteLine("You set out for " + Destination.Name + ", and as you do " + Origin.Name + " shrinks in the distance.");
            
            while(!(Progress == 0) || !(Progress == Distance))
            {
                if (Forward > 0)
                    Console.WriteLine(Distance - Progress + " miles to go.");
                else
                    Console.WriteLine(Progress + " miles to go.");
                bool chosen = false;
                while(!chosen)
                {
                    Console.WriteLine("(C)ontinue or (R)everse direction or (S)etup camp");
                    switch (Console.ReadLine().ToLower())
                    {
                        case "continue":
                        case "c":
                            Travel(1 * Forward);
                            chosen = true;
                            break;
                        case "reverse":
                        case "r":
                            Forward *= -1;
                            Travel(1 * Forward);
                            chosen = true;
                            break;
                        case "setup":
                        case "s":
                            Camp();
                            chosen = true;
                            break;
                        default:
                            chosen = false;
                            break;
                    }
                }
                    
            }

            if (Progress == 0)
                Origin.Event();
            else
                Destination.Event();
            
        }

        public void Travel(double miles)
        {
            Random rand = new Random();
            Console.WriteLine("Traveling to " + ((Forward > 0) ? Destination.Name : Origin.Name) + "...");
            Progress += Forward;
            if (rand.Next(1, 100) <= 5 * miles)
                new GameplayLoopCombat1.classes.Combat(Party.Members);
        }
        public void Camp()
        {
            Random rand = new Random();
            Console.WriteLine("You make camp for the night and attempt to secure your surrounds.");
            if(rand.Next(1, 100) < 33)
            {
                Console.WriteLine("As you lay down to sleep you hear a scream - it's an attack!");
                new GameplayLoopCombat1.classes.Combat(Party.Members);
            }
            else
            {
                Console.WriteLine("Your party restores 5 health each");
                Party.Members.ForEach(m => m.DoDamage(-5));
            }

        }
    }
}
