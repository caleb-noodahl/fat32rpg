
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading.Tasks;
using WebSocketSharp;
using MerchantRPG.Models.Engine.Combat;
using Newtonsoft.Json;
using MerchantRPG.Models.Engine.Multiplayer.Payloads;
using MerchantRPG.Models.Engine.GameObjects;

namespace MerchantRPG.Models.Engine.Multiplayer
{
    public static class Client
    {
        public static WebSocket ws { get; set; }
        public static async Task Start(string address)
        {
            Console.WriteLine("Connecting...");
            ws = new WebSocket("ws://" + address + "/msg");
         
         
            ws.OnMessage += (sender, e) =>
                ClientRoutes.CRouter(sender, e);

            ws.OnOpen += (sender, e) =>
            {
                Message msg = new Message("join game", "JoinGame", Party.MPPayload);
                ws.Send(JsonConvert.SerializeObject(msg));
            };

            ws.Connect();
            Console.WriteLine("Connected");
        }

        public static async Task SendState(bool joinGame = false)
        {
            if (!Party.Multiplayer)
                return;

            string endpoint = "ReceiveState";
            if (joinGame)
                endpoint = "JoinGame";
            Message msg = new Message("", endpoint, Party.MPPayload);
            if (Party.Host)
                Server.wss.WebSocketServices.BroadcastAsync(JsonConvert.SerializeObject(msg), () => {  });
            else
                ws.Send(JsonConvert.SerializeObject(msg));
        }

        public static async Task SendState(string desc)
        {
            if (!Party.Multiplayer)
                return;
            Message msg = new Message(desc, "ReceiveState", Party.MPPayload);
            if (Party.Host)
                Server.wss.WebSocketServices.BroadcastAsync(JsonConvert.SerializeObject(msg), () => { });
            else
                ws.Send(JsonConvert.SerializeObject(msg));
        }

        public static async Task SendMap(string desc = "")
        {
            if (!Party.Multiplayer)
                return;
            Message msg = new Message(desc, "ReceiveMap", new MapPayload(Map.towns, Party.MPID));
            if (Party.Host)
                Server.wss.WebSocketServices.BroadcastAsync(JsonConvert.SerializeObject(msg), () => { });
            else
                ws.Send(JsonConvert.SerializeObject(msg));
        }

    }
}