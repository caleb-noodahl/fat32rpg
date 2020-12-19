using GameplayLoopCombat1.classes;
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

namespace MerchantRPG
{
    class Program
    {

        static async Task Main(string[] args)
        {
            
            CharacterCreator cc = new CharacterCreator();
            Party.Lead = cc.result as PlayerState;
            //does this^ mean we're no longer passing the object by reference? if so, there will be a disconnect between the combat party entity and the playerstate entity.
            Party.Members.Add(cc.result);
            Map.GenerateTowns();
            
            var result = await Map.towns[0].Event(Party.Lead);
            Console.ReadLine();

        }


        
        
    }   
}
