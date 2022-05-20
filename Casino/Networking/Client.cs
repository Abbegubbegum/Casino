using System;
using System.Text.Json;
using Casino.Structures;
using WebSocketSharp;

namespace Casino.Networking
{
    public class Client
    {
        public WebSocket ws;

        public string ClientId { get; set; }

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
                Message msg = JsonSerializer.Deserialize<Message>(e.Data);

                if (ClientId == "" && msg.senderID == "")
                {
                    ClientId = msg.message;
                }
                else
                {

                }
            };

            ws.OnClose += (sender, e) =>
            {
                Console.WriteLine("Client disconnected from " + url);
            };

            ws.Connect();
        }

        public void Send(string msg)
        {
            Message m = new Message()
            {
                senderID = ClientId,
                message = msg
            };

            ws.Send(JsonSerializer.Serialize(m));
        }

        public void Disconnect()
        {
            ws.Close();
        }
    }
}