using Casino.Blackjack;
using WebSocketSharp.Server;
using Casino.Networking.WebsocketServices;
using Casino.Networking;

namespace Casino.Structures
{
    public class Gamehandler
    {
        public const string wsUrl = "ws://localhost:8080";

        WebSocketServer ws;

        BlackjackTable table;

        public Gamehandler()
        {
            SetupWSServer();

            Player p1 = new Player("albin");
            Client c = new Client(wsUrl + "/Blackjack", p1);

            ws.WebSocketServices.TryGetServiceHost("/Echo", out var host);

            host.Sessions.Broadcast("she");

            Console.ReadLine();

            // // Player p2 = new Player("bianc");

            table = new BlackjackTable();

            // table.AddPlayer(p1);
            // // table.AddPlayer(p2);

            // while (true)
            // {
            //     table.RunGame();
            // }
        }

        private void SetupWSServer()
        {
            ws = new WebSocketServer(wsUrl);

            ws.AddWebSocketService<Echo>("/Echo");
            ws.AddWebSocketService<Echo>("/Echo2");
            ws.AddWebSocketService<BlackjackService>("/Blackjack");

            ws.Start();
            Console.WriteLine($"Websocket server started at ws://localhost:8080");
        }
    }
}