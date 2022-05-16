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
            dealer = new BlackjackPlayer();
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

            if (players[0].GetBestValidCardValue() != 21 || players[0].extraCards.Count != 0)
            {
                while (dealer.GetBestValidCardValue() <= 16 && !players[0].IsBusted())
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
                if (dealer.IsBusted() && player.IsBusted())
                {
                    Console.WriteLine("Both busted, dealer win");
                }
                else if (dealer.GetBestValidCardValue() > player.GetBestValidCardValue() && !dealer.IsBusted() || player.IsBusted())
                {
                    Console.WriteLine("Dealer win");

                }
                else if (dealer.GetBestValidCardValue() < player.GetBestValidCardValue() && !player.IsBusted() || dealer.IsBusted())
                {
                    Console.WriteLine("Player Win");
                }
                else
                {
                    Console.WriteLine("Tie");
                }
                Console.WriteLine();
            }
            Console.ReadLine();

        }

        private void PerformPlayerTurn(BlackjackPlayer player)
        {
            Console.WriteLine("Your Hand: " + player.Hand);
            Console.WriteLine("Your Hand Value: " + player.GetBestValidCardValue());
            Console.WriteLine();
            Console.WriteLine("Dealer Card: " + dealer.Hand.card2);
            Console.WriteLine("Dealers Value: " + (dealer.Hand.card2.Value == CardValue.Ace ? 11 : dealer.Hand.card2.GetRawBlackjackValue()));
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

                        Console.WriteLine("Your Cards: ");
                        foreach (Card card in player.GetCards())
                        {
                            Console.WriteLine(card);
                        }
                        Console.WriteLine("Your value: " + player.GetBestValidCardValue());
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
            BlackjackPlayer bp = new()
            {
                Hand = p.Hand
            };

            players.Add(bp);
        }
    }
}