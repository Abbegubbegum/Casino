using Casino.enums;
using Casino.Objects;
using Casino.Structures;

namespace Casino.Blackjack
{
    public class BlackjackTable
    {
        private Deck deck;

        private BlackjackPlayer dealer;

        private List<BlackjackPlayer> players = new();

        public BlackjackTable()
        {
            deck = new Deck();
            dealer = new BlackjackPlayer("Dealer");
        }

        public void RunGame()
        {
            foreach (BlackjackPlayer p in players)
            {
                p.Hand = deck.DealHand();
                // p.Hand = new Hand(new Card(Suit.Diamond, CardValue.Ace), new Card(Suit.Diamond, CardValue.Queen));
            }

            dealer.Hand = deck.DealHand();
            // dealer.Hand = new Hand(new Card(Suit.Diamond, CardValue.Ace), new Card(Suit.Diamond, CardValue.Queen));

            foreach (var player in players)
            {
                PerformPlayerTurn(player);
            }

            Console.WriteLine("Dealer Hand: " + dealer.Hand);
            Console.WriteLine("Dealers Value: " + dealer.GetBestValidCardValue());
            Console.ReadLine();

            bool allPlayersHaveBlackJack = players.FindAll(p => !p.HasAnAce() || p.extraCards.Count != 0).Count == 0;
            bool allPlayersAreBust = players.FindAll(p => !p.IsBusted()).Count == 0;

            if (!allPlayersAreBust || !allPlayersHaveBlackJack)
            {
                while (dealer.GetBestValidCardValue() <= 16)
                {
                    Console.WriteLine("Dealer takes a new card");
                    Console.WriteLine();
                    dealer.extraCards.Add(deck.DealCard());

                    Console.WriteLine("Dealer Cards: ");
                    foreach (Card card in dealer.GetCards())
                    {
                        Console.WriteLine(card);
                    }
                    Console.WriteLine("Dealer value: " + dealer.GetBestValidCardValue());

                    Console.ReadLine();
                }
            }

            foreach (var player in players)
            {
                ShowResult(player);
            }
            Console.ReadLine();

        }

        private void ShowResult(BlackjackPlayer player)
        {
            Console.WriteLine(player.Name + ": ");
            if (dealer.IsBusted() && player.IsBusted())
            {
                Console.WriteLine("Both busted, you lose");
            }
            else if (dealer.GetBestValidCardValue() > player.GetBestValidCardValue() && !dealer.IsBusted() || player.IsBusted())
            {
                Console.WriteLine("you lose");

            }
            else if (dealer.GetBestValidCardValue() < player.GetBestValidCardValue() && !player.IsBusted() || dealer.IsBusted())
            {
                Console.WriteLine("you win");
            }
            else
            {
                Console.WriteLine("stand, its a draw");
            }
            Console.WriteLine();
        }

        private void PerformPlayerTurn(BlackjackPlayer player)
        {
            Console.WriteLine(player.Name + "'s Hand: " + player.Hand);
            Console.WriteLine(player.Name + "'s Hand Value: " + player.GetBestValidCardValue());
            Console.WriteLine();
            Console.WriteLine("Dealer Card: " + dealer.Hand.card2);
            Console.WriteLine("Dealer's Value: " + (dealer.Hand.card2.Value == CardValue.Ace ? 11 : dealer.Hand.card2.GetRawBlackjackValue()));
            Console.WriteLine();

            string? playerInput = "";

            if (player.GetBestValidCardValue() != 21 && dealer.GetBestValidCardValue() != 21)
            {
                while (playerInput != "2" && !player.IsBusted())
                {
                    Console.WriteLine("What you do, Hit=1 Stay=2");
                    playerInput = Console.ReadLine();
                    if (playerInput == "1")
                    {
                        player.extraCards.Add(deck.DealCard());

                        Console.WriteLine(player.Name + "'s Cards: ");
                        foreach (Card card in player.GetCards())
                        {
                            Console.WriteLine(card);
                        }
                        Console.WriteLine(player.Name + "'s value: " + player.GetBestValidCardValue());
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("BLACKJACK");
                Console.ReadLine();
            }
        }

        public void AddPlayer(Player p)
        {
            BlackjackPlayer bp = new(p);

            players.Add(bp);
        }
    }
}