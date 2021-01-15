using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Multiplayer
{
    public static class MultiPlayer
    {
        public static async Task Start()
        {
            Console.WriteLine("Start multiplayer? (y/n)");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Do you want to host? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    await Server.Start();
                }
                else
                {
                    Console.WriteLine("Enter the IP:PORT of the host");
                    await Client.Start(Console.ReadLine());
                }
            }

        }
    }
}
