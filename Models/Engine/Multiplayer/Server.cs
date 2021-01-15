
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using MerchantRPG.Models.Engine.Combat;

namespace MerchantRPG.Models.Engine.Multiplayer
{
    public static class Server
    {
        public static int port { get; set; } = 4422;
        public static WebSocketServer wss { get; set; }

        public static async Task Start()
        {
            Console.WriteLine("Starting Multiplayer Host...");
            wss = new WebSocketServer(port);
            wss.AddWebSocketService<Router>("/msg");
            wss.Start();
            Party.Multiplayer = true;
            Party.Host = true;
            Console.WriteLine("Multiplayer waiting for connections at 127.0.0.1:" + port);
        }

        public static async Task Stop()
        {
            Console.WriteLine("Stopping Multiplayer Host");
            wss.Stop();
        }
    }
}