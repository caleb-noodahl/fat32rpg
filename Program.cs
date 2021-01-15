using MerchantRPG.Models.Engine.Combat;
using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine;
using MerchantRPG.Models.Engine.Events;
using MerchantRPG.Models.Engine.GameObjects;
using MerchantRPG.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MerchantRPG.Models.Engine.Multiplayer;

namespace MerchantRPG
{
    class Program
    {

        static async Task Main(string[] args)
        {
            CharacterCreator cc = new CharacterCreator();
            Party.Lead = cc.result;
            Party.Members.Add(cc.result);
            await MultiPlayer.Start();
            Console.WriteLine("---continue---");
            Console.ReadLine();
            if (!Party.Multiplayer || Party.Host)
                Map.GenerateTowns();
            var result = await Map.towns[0].Event();
            Console.ReadLine();

        }
        
        
    }   
}
