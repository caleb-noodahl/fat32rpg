using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine
{
    public class GameLog
    {
        public DateTime Time { get; set; } = DateTime.Now; 
        public string Msg { get; set; }
        public List<object> GameObjects { get; set; } = new List<object>();
        public bool HasError = false;
    }
}
