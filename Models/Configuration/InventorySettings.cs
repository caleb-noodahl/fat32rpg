using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Configuration
{
    public class InventorySettings
    {
        public List<InventoryItem> Default { get; set; } = new List<InventoryItem>(); 
        public InventorySettings() { }
        public InventorySettings(List<InventoryItem> d)
        {
            Default = d;
        }

    }
}
