using Casino.Blackjack;
using Casino.Structures;
using WebSocketSharp.Server;
using Casino.Networking.WebsocketServices;

const string wsUrl = "ws://localhost:8080";

var ws = new WebSocketServer(wsUrl);

ws.AddWebSocketService<Echo>("/Echo");
ws.AddWebSocketService<Echo>("/Echo2");

ws.Start();
Console.WriteLine($"Websocket server started at ws://localhost:8080");

Console.ReadLine();
ws.WebSocketServices.TryGetServiceHost("/Echo", out var host);

host.Sessions.Broadcast("she");

ws.Stop();

Console.WriteLine("Stopped");
Console.ReadLine();


// Player p1 = new Player("albin");
// // Player p2 = new Player("bianc");

// BlackjackTable table = new BlackjackTable();

// table.AddPlayer(p1);
// // table.AddPlayer(p2);

// while (true)
// {
//     table.RunGame();
// }
