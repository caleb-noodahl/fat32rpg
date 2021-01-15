using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace MerchantRPG.Models.Engine.Multiplayer
{
    public class Message
    {
        public string Call { get; set; }
        public string Msg { get; set; }
        public dynamic Payload { get; set; }

        public Message(string msg = "", string call = "", dynamic payload = null)
        {
            Msg = msg;
            Call = call;
            Payload = payload;
        }
    }
}
