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

        public BlackjackTable(Deck d)
        {
            deck = d;
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

            Console.WriteLine("Your Hand: " + players[0].Hand);
            Console.WriteLine("Your Hand Value: " + players[0].GetBestValidCardValue());
            Console.WriteLine();
            Console.WriteLine("Dealer Card: " + dealer.Hand.card2);
            Console.WriteLine("Dealers Value: " + dealer.Hand.card2.GetRawBlackjackValue());

            string? playerInput = "";

            if (players[0].GetBestValidCardValue() != 21 && dealer.GetBestValidCardValue() != 21)
            {
                while (playerInput != "2" && !players[0].IsBusted())
                {
                    Console.WriteLine();
                    Console.WriteLine("What you do, Hit=1 Stay=2");
                    playerInput = Console.ReadLine();
                    if (playerInput == "1")
                    {
                        players[0].extraCards.Add(deck.DealCard());

                        Console.WriteLine("Your Cards: ");
                        foreach (Card card in players[0].GetCards())
                        {
                            Console.WriteLine(card);
                        }
                        Console.WriteLine("Your value: " + players[0].GetBestValidCardValue());

                    }
                }
            }

            Console.WriteLine();
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

            if (dealer.IsBusted() && players[0].IsBusted())
            {
                Console.WriteLine("Both busted, dealer win");
            }
            else if (dealer.GetBestValidCardValue() > players[0].GetBestValidCardValue() && !dealer.IsBusted() || players[0].IsBusted())
            {
                Console.WriteLine("Dealer win");

            }
            else if (dealer.GetBestValidCardValue() < players[0].GetBestValidCardValue() && !players[0].IsBusted() || dealer.IsBusted())
            {
                Console.WriteLine("Player Win");
            }
            else
            {
                Console.WriteLine("Tie");
            }
            Console.ReadLine();

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