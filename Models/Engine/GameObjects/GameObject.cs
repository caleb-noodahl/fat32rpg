using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine
{
    public class GameObject
    {
        public double Value { get; set; }
        //public int Weight { get; set; }
        public bool IsInventory { get; set; }
        public bool Interactable { get; set; }
        public bool Rating { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


    }

    
}
