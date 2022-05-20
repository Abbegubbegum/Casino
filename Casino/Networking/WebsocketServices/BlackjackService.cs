using WebSocketSharp;
using WebSocketSharp.Server;
using Casino.Structures;
using Casino.Blackjack;

namespace Casino.Networking.WebsocketServices
{
    public class BlackjackService : WebSocketBehavior
    {
        static List<string> currentIds = new();

        public BlackjackTable table;

        protected override void OnOpen()
        {
            var newIDs = Sessions.IDs;

            foreach (var id in newIDs)
            {
                if (!currentIds.Contains(id))
                {
                    Console.WriteLine($"BLACKJACK NEW ID + {id}");
                    Gamehandler.AddPlayerWithID(id);
                    currentIds.Add(id);
                }
            }

            // var differentIDs = from id in newIDs
            //                    where !ids.Contains(id)
            //                    select id;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            table.RecieveMessage(e.Data);
        }
    }
}