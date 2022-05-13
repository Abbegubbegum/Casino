using Casino.enums;

namespace Casino.Objects
{
    public class Deck
    {
        public List<Card> cards = new(52);

        private static Random r = new();

        public Deck()
        {
            ResetCards();
        }

        public void ResetCards()
        {
            if (cards.Count > 0)
                cards.RemoveRange(0, cards.Count - 1);

            for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
            {
                for (int j = 1; j < Enum.GetNames(typeof(CardValue)).Length; j++)
                {
                    cards.Add(new Card((Suit)i, (CardValue)j));
                }
            }
        }

        public Card DealCard()
        {
            if (cards.Count == 0)
                throw new Exception("Cards Empty");

            int i = r.Next(cards.Count);
            Card c = cards[i];
            cards.RemoveAt(i);
            return c;
        }

        public Hand DealHand()
        {
            return new Hand(DealCard(), DealCard());
        }
    }
}