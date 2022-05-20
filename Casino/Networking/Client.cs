using System;
using Casino.Structures;
using WebSocketSharp;

namespace Casino.Networking
{
    public class Client
    {
        public WebSocket ws;

        Player p;

        public Client(string url)
        {
            // this.p = p;

            ws = new WebSocket(url);

            ws.OnOpen += (sender, e) =>
            {
                Console.WriteLine("Client Connected to " + url);
            };

            // ws.OnMessage += p.OnMessage;

            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine("Client received message: " + e.Data);
            };

            ws.OnClose += (sender, e) =>
            {
                Console.WriteLine("Client disconnected from " + url);
            };

            ws.Connect();
        }

        public void Send(string msg)
        {
            ws.Send(msg);
        }

        public void Disconnect()
        {
            ws.Close();
        }
    }
}