using Casino.Blackjack;
using WebSocketSharp.Server;
using Casino.Networking.WebsocketServices;
using Casino.Networking;

namespace Casino.Structures
{
    public class Gamehandler
    {
        // public const string wsUrl = "ws://10.151.169.33:8080";
        public const string wsUrl = "ws://localhost:8080";

        WebSocketServer ws;

        BlackjackTable table;

        static List<Player> players = new();

        public Gamehandler()
        {
            table = new BlackjackTable();
            SetupWSServer();

            // Client c = new Client(wsUrl + "/Blackjack");

            ws.WebSocketServices.TryGetServiceHost("/Blackjack", out table.bjHost);
            // Player p2 = new Player("bianc");

            Console.WriteLine("Waiting on players");
            while (players.Count == 0) ;

            Console.ReadLine();
            Console.WriteLine("Starting game");


            foreach (var p in players)
            {
                table.AddPlayer(p);
            }// // table.AddPlayer(p2);

            while (true)
            {
                table.RunGame();
            }
        }

        private void SetupWSServer()
        {
            ws = new WebSocketServer(wsUrl);

            ws.AddWebSocketService<Echo>("/Echo");
            ws.AddWebSocketService<Echo>("/Echo2");
            ws.AddWebSocketService<BlackjackService>("/Blackjack", b => b.table = table);

            ws.Start();
            Console.WriteLine($"Websocket server started at ws://localhost:8080");
        }

        public static void AddPlayerWithID(string id)
        {
            Console.WriteLine("Adding player to players");
            Player p = new Player("who");
            p.ID = id;

            players.Add(p);
        }
    }
}