using Casino.enums;
using Casino.Objects;

namespace Casino.Structures
{
    public class BlackjackPlayer : Player
    {
        public List<Card> extraCards = new();

        public bool blackjack = false;

        public int currentBet = 0;

        public BlackjackPlayer(string name) : base(name)
        {
        }

        public BlackjackPlayer(Player p) : base(p.Name)
        {
            Hand = p.Hand;
            Balance = p.Balance;
            ID = p.ID;
        }

        public void Reset()
        {
            extraCards.RemoveRange(0, extraCards.Count);
            blackjack = false;
            currentBet = 0;
        }

        public int GetRawCardValue()
        {
            int value = 0;

            value += Hand.card1.GetRawBlackjackValue();
            value += Hand.card2.GetRawBlackjackValue();
            foreach (Card card in extraCards)
            {
                value += card.GetRawBlackjackValue();
            }

            return value;
        }

        public int GetBestValidCardValue()
        {
            int value = GetRawCardValue();

            if (HasAnAce())
            {
                if (value + 10 <= 21)
                {
                    value += 10;
                }
            }

            return value;
        }

        public bool IsBusted()
        {
            return GetBestValidCardValue() > 21;
        }

        public List<Card> GetCards()
        {
            List<Card> cards = new()
            {
                Hand.card1,
                Hand.card2
            };

            cards.AddRange(extraCards);

            return cards;
        }

        public bool HasAnAce()
        {
            return GetCards().FindAll(i => i.Value == CardValue.Ace).Count > 0;
        }

        public bool IsSoft()
        {
            return GetRawCardValue() != GetBestValidCardValue();
        }

        public string GetValueToString()
        {
            string value = GetBestValidCardValue().ToString();
            if (IsSoft())
                value = string.Concat("Soft ", value);

            return value;
        }
    }

}