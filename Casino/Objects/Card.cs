using Casino.enums;

namespace Casino.Objects
{
    public struct Card
    {
        public Suit Suit { get; init; }
        public CardValue Value { get; init; }

        public Card(Suit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public override string? ToString()
        {
            return $"{Value} of {Suit}";
        }

        public int GetRawBlackjackValue()
        {
            int numberValue = (int)Value;
            if (numberValue >= 10 && numberValue <= 13)
            {
                return 10;
            }
            else
            {
                return numberValue;
            }
        }
    }
}