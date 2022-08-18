using Casino.Blackjack;
using WebSocketSharp.Server;
using Casino.Networking.WebsocketServices;
using Casino.Networking;

namespace Casino.Structures
{
    public class ServerHandler
    {
        // public const string wsUrl = "ws://10.151.169.33:8080";
        public const string wsUrl = "ws://localhost:8080";

        WebSocketServer ws;

        BlackjackTable table;

        static List<Player> players = new();

        public ServerHandler()
        {
            table = new BlackjackTable();
            SetupWSServer();

            ws.WebSocketServices.TryGetServiceHost("/Blackjack", out table.bjHost);
            Activate();
        }

        public void Activate()
        {
            if (players.Count == 0)
            {
                Console.WriteLine("Waiting for a player");
                while (players.Count == 0) ;
            }

            foreach (var p in players)
            {
                table.AddPlayer(p);
            }

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