using Casino.Objects;

namespace Casino.Structures
{
    public class Player
    {
        public Hand Hand { get; set; }
        public string Name { get; init; }

        public int Balance { get; set; }

        public Player(string name, int balance = 100)
        {
            Name = name;
            Balance = balance;
        }
    }
}