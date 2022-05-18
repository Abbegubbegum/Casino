using Casino.Networking;
using Casino.Objects;
using WebSocketSharp;

namespace Casino.Structures
{
    public class Player
    {
        public Hand Hand { get; set; }
        public string Name { get; init; }

        public int Balance { get; set; }

        public string ID { get; set; }

        public Player(string name, int balance = 100)
        {
            Name = name;
            Balance = balance;
        }

        public void OnMessage(object? sender, MessageEventArgs e)
        {
            string text = e.Data;
            Console.WriteLine($"Message received from server to {Name}: {e.Data}");
        }
    }
}