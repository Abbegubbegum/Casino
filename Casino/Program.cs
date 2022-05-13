using Casino.Blackjack;
using Casino.Objects;
using Casino.Structures;
while (true)
{
    Deck d = new Deck();

    Player p = new Player();

    BlackjackTable table = new BlackjackTable(d);

    table.AddPlayer(p);

    table.RunGame();
}
