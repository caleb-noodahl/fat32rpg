using MerchantRPG.Models.Configuration;
using MerchantRPG.Models.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine.GameObjects
{
    public static class Map
    {
        public static List<TownEvent> towns { get; set; } = new List<TownEvent>();

        public static List<TownEvent> GenerateTowns()
        {
            List<TownEvent> response = new List<TownEvent>();

            Dictionary<TownDefintion, InventorySettings> _towns = new Dictionary<TownDefintion, InventorySettings>();
            List<TownDefintion> _townDefs = new List<TownDefintion>();
            var rand = new Random();

            //generate a x number of towns
            for (int i = 0; i < rand.Next(2, 6); i++)
            {
                TownDefintion town = new TownDefintion()
                {
                    Name = i.ToString(),
                    Rating = i * rand.Next(0, 3),
                    Distance = i * rand.Next(1, 300)
                };


                List<InventoryItem> townItems = new List<InventoryItem>();
                for (var inventoryIterator = 0; inventoryIterator < rand.Next(1, 7); inventoryIterator++)
                {
                    townItems.Add(new InventoryItem(true));
                }
                _townDefs.Add(town);
                _towns.Add(town, new InventorySettings(townItems));
            }
            foreach (KeyValuePair<TownDefintion, InventorySettings> val in _towns)
            {
                response.Add(new TownEvent(val.Key.Name, new TownSettings(_townDefs), val.Value));
            }

            towns = response;
            return response;
        }
    }

}
