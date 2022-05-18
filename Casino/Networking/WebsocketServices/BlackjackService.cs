using WebSocketSharp;
using WebSocketSharp.Server;
using System.Linq;

namespace Casino.Networking.WebsocketServices
{
    public class BlackjackService : WebSocketBehavior
    {
        IEnumerable<string> ids = Enumerable.Empty<string>();

        protected override void OnOpen()
        {
            var newIDs = Sessions.IDs;

            foreach (var id in newIDs)
            {
                if (!ids.Contains(id))
                {
                    Console.WriteLine($"BLACKJACK NEW ID + {id}");
                }
            }

            // var differentIDs = from id in newIDs
            //                    where !ids.Contains(id)
            //                    select id;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            
        }
    }
}