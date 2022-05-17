using Casino.Blackjack;
using Casino.Structures;

Player p1 = new Player("albin");
// Player p2 = new Player("bianc");

BlackjackTable table = new BlackjackTable();

table.AddPlayer(p1);
// table.AddPlayer(p2);

while (true)
{
    table.RunGame();
}
