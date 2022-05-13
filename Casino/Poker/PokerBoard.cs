using Casino.Objects;

namespace Casino.Poker
{
    public class PokerBoard
    {
        public Card?[] cards = new Card?[5];

        private Deck deck;

        public PokerBoard(Deck d)
        {
            deck = d;
        }


        public void DealNextAction()
        {
            if (cards[0] == null)
            {
                DealFlop();
            }
            else if (cards[3] == null)
            {
                DealTurn();
            }
            else if (cards[4] == null)
            {
                DealRiver();
            }
            else
            {
                Console.WriteLine("Board Finished");
            }
        }

        private void DealFlop()
        {
            cards[0] = deck.DealCard();
            cards[1] = deck.DealCard();
            cards[2] = deck.DealCard();
        }

        private void DealTurn()
        {
            cards[3] = deck.DealCard();
        }

        private void DealRiver()
        {
            cards[4] = deck.DealCard();
        }

        public override string? ToString()
        {
            return $"{cards[0]} - {cards[1]} - {cards[2]} - {cards[3]} - {cards[4]}";
        }
    }
}