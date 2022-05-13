using Casino.Blackjack;
using Casino.Objects;
using Casino.Structures;
while (true)
{
    Deck d = new Deck();

    Player p1 = new Player();
    Player p2 = new Player();

    BlackjackTable table = new BlackjackTable();

    table.AddPlayer(p1);
    table.AddPlayer(p2);

    table.RunGame();
}
