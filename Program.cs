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
        static string[] names = new string[]
        {
            "glasses", "sword", "plate", "frizbee", "tongs", "hammer"
        };
        static string[] towns = new string[]
        {
            "here", "there", "necropolis", "bastion"
        };
        static async Task Main(string[] args)
        {
            PlayerState charct = new Character("gnome", 1, 4, 6) as PlayerState; 


            var towns = GenerateTowns(); 


        }


        private static List<TownEvent> GenerateTowns()
        {
            List<TownEvent> response = new List<TownEvent>();
            var rand = new Random(); 
            
            //generate a x number of towns
            for(int i = 0; i < rand.Next(1, 3); i++)
            {
                List<InventoryItem> townItems = new List<InventoryItem>(); 
                for(var inventoryIterator = 0; inventoryIterator < rand.Next(1, 7); inventoryIterator++)
                {
                    townItems.Add(new InventoryItem()
                    {
                        StatModifier = rand.Next(0, 4),
                        Stat = (Stats)rand.Next(0, 3),
                        Name = names[rand.Next(0, names.Length -1)],
                        Weight = rand.Next(1, 20),
                        Value = (double)rand.Next(0, 300),
                        Type = (ItemType)rand.Next(0, 2)
                    });
                }
                
            }
            return response;
        }
        
    }   
}
