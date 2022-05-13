using Casino.Objects;
using Casino.Structures;

namespace Casino.Poker
{
    public class PokerTable
    {
        public List<Player> players = new();

        private Deck deck;

        private PokerBoard board;

        public PokerTable()
        {
            deck = new Deck();

            board = new PokerBoard(deck);
        }

        public void RunGame()
        {
            //Deal cards to players
            foreach (Player p in players)
            {
                p.Hand = deck.DealHand();
            }

            for (int i = 0; i < 3; i++)
            {
                RunBettingRound();
                board.DealNextAction();
            }

            RunBettingRound();
        }

        private void RunBettingRound()
        {
            throw new NotImplementedException();
        }

        public void AddPlayer(Player p)
        {
            players.Add(p);
        }

    }
}