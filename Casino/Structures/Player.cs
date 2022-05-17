using Casino.Objects;

namespace Casino.Structures
{
    public class Player
    {
        public Hand Hand { get; set; }
        public string Name { get; init; }

        public Player(string name)
        {
            Name = name;
        }
    }
}