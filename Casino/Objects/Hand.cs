namespace Casino.Objects
{
    public struct Hand
    {
        public Card card1;
        public Card card2;

        public Hand(Card card1, Card card2)
        {
            this.card1 = card1;
            this.card2 = card2;
        }

        public override string? ToString()
        {
            return $"{card1} and {card2}";
        }
    }
}