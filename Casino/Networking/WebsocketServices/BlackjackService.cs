using WebSocketSharp;
using WebSocketSharp.Server;
using Casino.Structures;
using Casino.Blackjack;
using System.Text.Json;

namespace Casino.Networking.WebsocketServices
{
    public class BlackjackService : WebSocketBehavior
    {
        static List<string> currentIds = new();

        public BlackjackTable table;

        protected override void OnOpen()
        {

            if (!currentIds.Contains(ID))
            {
                Console.WriteLine($"BLACKJACK NEW ID + {ID}");
                ServerHandler.AddPlayerWithID(ID);
                currentIds.Add(ID);
                Message msg = new Message()
                {
                    type = "ID-ACK",
                    data = ID
                };
                Send(JsonSerializer.Serialize(msg));
            }
        }

        // var differentIDs = from id in newIDs
        //                    where !ids.Contains(id)
        //                    select id;


        protected override void OnMessage(MessageEventArgs e)
        {

            table.RecieveMessage(JsonSerializer.Deserialize<Message>(e.Data));
        }


    }
}