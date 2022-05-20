using Casino.enums;
using Casino.Objects;
using Casino.Structures;
using WebSocketSharp.Server;

namespace Casino.Blackjack
{
    public class BlackjackTable
    {
        private Deck deck;

        private BlackjackPlayer dealer;

        private List<BlackjackPlayer> players = new();

        public WebSocketServiceHost bjHost;

        private Queue<string> messages = new Queue<string>();

        public BlackjackTable()
        {
            deck = new Deck();
            dealer = new BlackjackPlayer("Dealer");
        }

        public void RunGame()
        {
            foreach (var p in players)
            {
                p.Reset();

                GetPlayerBet(p);
            }

            foreach (BlackjackPlayer p in players)
            {
                p.Hand = deck.DealHand();
                // p.Hand = new Hand(new Card(Suit.Diamond, CardValue.Ace), new Card(Suit.Diamond, CardValue.Five));
            }

            dealer.Reset();
            dealer.Hand = deck.DealHand();
            // dealer.Hand = new Hand(new Card(Suit.Diamond, CardValue.Ace), new Card(Suit.Diamond, CardValue.Queen));

            if (dealer.GetBestValidCardValue() == 21)
            {
                dealer.blackjack = true;
                BroadcastMessage("DEALER BLACKJACK");

            }
            else
            {
                foreach (var player in players)
                {
                    PerformPlayerTurn(player);
                }
            }


            BroadcastMessage($"Dealer Hand: {dealer.Hand}");
            BroadcastMessage($"Dealers Value: {dealer.GetValueToString()}");

            var playersThatNeedTheDealerPlay = from player in players
                                               where !player.blackjack && !player.IsBusted()
                                               select player;


            if (playersThatNeedTheDealerPlay.Any())
            {
                while (dealer.GetBestValidCardValue() <= 16)
                {
                    BroadcastMessage("Dealer takes a new card");
                    dealer.extraCards.Add(deck.DealCard());

                    BroadcastMessage("Dealer Cards: ");
                    foreach (Card card in dealer.GetCards())
                    {
                        BroadcastMessage(card.ToString());
                    }
                    BroadcastMessage($"Dealer value: {dealer.GetValueToString()}");

                    GetMessage();
                }
            }

            foreach (var player in players)
            {
                ShowResult(player);
            }
            GetMessage();

        }

        private void GetPlayerBet(BlackjackPlayer player)
        {
            SendMessageToPlayer("How much you betting?", player);
            SendMessageToPlayer($"Balance: {player.Balance}", player);

            bool parseSuccess = int.TryParse(GetMessage(), out int input);
            while (!parseSuccess || input < 0 || input > player.Balance)
            {
                SendMessageToPlayer("Try again", player);
                parseSuccess = int.TryParse(GetMessage(), out input);
            }

            player.currentBet = input;
            player.Balance -= input;
        }

        private void PerformPlayerTurn(BlackjackPlayer player)
        {
            SendMessageToPlayer($"{player.Name}'s Hand: {player.Hand}", player);
            SendMessageToPlayer($"{player.Name}'s Hand Value: {player.GetValueToString()}", player);

            SendMessageToPlayer($"Dealer Card: {dealer.Hand.card2}", player);
            SendMessageToPlayer($"Dealer's Value: {(dealer.Hand.card2.Value == CardValue.Ace ? 11 : dealer.Hand.card2.GetRawBlackjackValue())}", player);

            string? playerInput = "";

            if (player.GetBestValidCardValue() != 21 && dealer.GetBestValidCardValue() != 21)
            {
                while (playerInput != "2" && !player.IsBusted())
                {
                    SendMessageToPlayer("What you do, Hit=1 Stay=2", player);
                    playerInput = GetMessage();
                    if (playerInput == "1")
                    {
                        player.extraCards.Add(deck.DealCard());

                        SendMessageToPlayer($"{player.Name}'s Cards: ", player);
                        foreach (Card card in player.GetCards())
                        {
                            SendMessageToPlayer(card.ToString(), player);
                        }
                        SendMessageToPlayer($"{player.Name}'s value: {player.GetValueToString()}", player);
                        // GetMessage();
                    }
                }
            }
            else
            {
                player.blackjack = true;
                SendMessageToPlayer("BLACKJACK", player);
                GetMessage();
            }
        }

        private void ShowResult(BlackjackPlayer player)
        {
            SendMessageToPlayer($"{player.Name}: ", player);
            if (dealer.IsBusted() && player.IsBusted())
            {
                SendMessageToPlayer("Both busted, you lose", player);
            }
            else if (dealer.GetBestValidCardValue() > player.GetBestValidCardValue() && !dealer.IsBusted() || player.IsBusted())
            {
                SendMessageToPlayer("you lose", player);

            }
            else if (dealer.GetBestValidCardValue() < player.GetBestValidCardValue() && !player.IsBusted() || dealer.IsBusted())
            {
                SendMessageToPlayer("you win", player);
                player.currentBet *= 2;
                player.Balance += player.currentBet;
            }
            else
            {
                SendMessageToPlayer("stand, its a draw", player);
                player.Balance += player.currentBet;
            }
        }

        public void AddPlayer(Player p)
        {
            BlackjackPlayer bp = new(p);

            players.Add(bp);
        }

        private void SendMessageToPlayer(string msg, Player p)
        {
            Console.WriteLine("Sending message to player " + msg);
            bjHost.Sessions.SendTo(msg, p.ID);
        }

        private void BroadcastMessage(string msg)
        {
            bjHost.Sessions.Broadcast(msg);
        }

        public void RecieveMessage(string msg)
        {
            Console.WriteLine("Table recieved message: " + msg);
            messages.Enqueue(msg);
        }

        public string GetMessage()
        {
            while (messages.Count <= 0) ;

            return messages.Dequeue();
        }
    }
}