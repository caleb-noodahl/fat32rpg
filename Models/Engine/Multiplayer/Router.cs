using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;
using MerchantRPG.Models.Engine.Multiplayer.Payloads;
using MerchantRPG.Models.Engine.Combat;
using System.Linq;
using MerchantRPG.Models.Engine.GameObjects;

namespace MerchantRPG.Models.Engine.Multiplayer
{
    public class Router : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Got message: " + e.ToString());

            Message data = JsonConvert.DeserializeObject<Message>(e.Data);
            if (data == null || data.Call == null)
            {
                Console.WriteLine("Bad message " + e.ToString());
                Send("Bad message " + e.ToString());
            }

            var meth = this.GetType().GetMethod(data.Call);
            object[] objs = new object[]
            {
                data
            };
            if (meth != null)
                meth.Invoke(this, objs);
            else
            {
                Console.WriteLine("No call " + e.ToString());
                Send("No call " + e.ToString());
            }
        }

        protected override void OnOpen()
        {
            Message msg = new Message("Loaded " + Party.Lead.Name + "'s Map", "ReceiveMap", new MapPayload(Map.towns, Party.MPID));
            Send(JsonConvert.SerializeObject(msg));
        }




        public void ReceiveMap(Message data)
        {
            Console.WriteLine(data.Msg);
            MapPayload payload = JsonConvert.DeserializeObject<MapPayload>(data.Payload.ToString());
            Map.towns = payload.Towns;
            Sessions.Broadcast(JsonConvert.SerializeObject(data));
        }

        public  void ReceiveState(Message data)
        {
            Console.WriteLine(data.Msg);
            PartyPayload payload = JsonConvert.DeserializeObject<PartyPayload>(data.Payload.ToString());
            Party.MultiMembers.RemoveAll(p => p.MPID.ToString() == payload.MPID.ToString());
            Party.MultiMembers.Add(payload);
            Sessions.Broadcast(JsonConvert.SerializeObject(data));
        }

        public  void JoinGame(Message data)
        {
            Console.WriteLine(data.Msg);
            PartyPayload payload = JsonConvert.DeserializeObject<PartyPayload>(data.Payload.ToString());
            Sessions.Broadcast(JsonConvert.SerializeObject(data));
            Party.MultiMembers.ForEach(p => 
            {
                Message msg = new Message("join game", "JoinGame", p);
                Send(JsonConvert.SerializeObject(msg));
            });
            Party.MultiMembers.Add(payload);

        }
    }
    
    public static class ClientRoutes 
    {
        public static object CRouter(object? sender, MessageEventArgs e)
        {
            Console.WriteLine("Got message: " + e.ToString());

            Message data = JsonConvert.DeserializeObject<Message>(e.Data);
            if (data == null || data.Call == null)
            {
                Console.WriteLine("Bad message " + e.ToString());
                return new Message("Bad message " + e.ToString());
            }


            var meth = Type.GetType("MerchantRPG.Models.Engine.Multiplayer.ClientRoutes").GetMethod(data.Call);
            object[] objs = new object[]
            {
                data
            };
            if (meth != null)
                return meth.Invoke(null, objs);
            else
            {
                Console.WriteLine("No call " + e.ToString());
                return new Message("No call " + e.ToString());
            }
        }

        public static void ReceiveMap(Message data)
        {
            Console.WriteLine(data.Msg);
            MapPayload payload = JsonConvert.DeserializeObject<MapPayload>(data.Payload.ToString());
            if (payload.MPID != Party.MPID)
            {
                Map.towns = payload.Towns;
            }
        }


        public static void ReceiveState(Message data)
        {
            Console.WriteLine(data.Msg);
            PartyPayload payload = JsonConvert.DeserializeObject<PartyPayload>(data.Payload.ToString());
            if (payload.MPID != Party.MPID)
            {
                Party.MultiMembers.RemoveAll(p => p.MPID.ToString() == payload.MPID.ToString());
                Party.MultiMembers.Add(payload);
            }
        }

        public static void JoinGame(Message data)
        {
            Console.WriteLine(data.Msg);
            PartyPayload payload = JsonConvert.DeserializeObject<PartyPayload>(data.Payload.ToString());
            if(payload.MPID != Party.MPID && !Party.MultiMembers.Exists(p => p.MPID == payload.MPID))
                Party.MultiMembers.Add(payload);
        }

    }
}

