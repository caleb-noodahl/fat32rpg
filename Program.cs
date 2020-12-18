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
            PlayerState c = new PlayerState("gnome", 1, 4, 6);
            c.Currency = 1000;
            c.Capacity = 100;

            var towns = GenerateTowns();
            var result = await towns[0].Event(c);
            Console.ReadLine();

        }


        private static List<TownEvent> GenerateTowns()
        {
            List<TownEvent> response = new List<TownEvent>();
            
            Dictionary<TownDefintion, InventorySettings> _towns = new Dictionary<TownDefintion, InventorySettings>();
            List<TownDefintion> _townDefs = new List<TownDefintion>(); 
            var rand = new Random(); 
            
            //generate a x number of towns
            for(int i = 0; i < rand.Next(2, 6); i++)
            {
                TownDefintion town = new TownDefintion()
                {
                    Name = i.ToString(),
                    Rating = i * rand.Next(0, 3),
                    Distance = i * rand.Next(1, 300)
                };
                
                
                List<InventoryItem> townItems = new List<InventoryItem>(); 
                for(var inventoryIterator = 0; inventoryIterator < rand.Next(1, 7); inventoryIterator++)
                {
                    townItems.Add(new InventoryItem(true)); 
                }
                _townDefs.Add(town); 
                _towns.Add(town, new InventorySettings(townItems));
            }
            foreach(KeyValuePair<TownDefintion, InventorySettings> val in _towns)
            {
                response.Add(new TownEvent(val.Key.Name, new TownSettings(_townDefs), val.Value));
            }


            
            return response;
        }
        
    }   
}
