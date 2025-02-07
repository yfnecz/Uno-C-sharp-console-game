namespace Uno.tests
{
    public class TestDeck : Deck
    {
        public TestDeck()
        {
            newDeck = new Stack<Card>();
            discardPile = new Stack<Card>();
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.DarkYellow };
            foreach (ConsoleColor color in colors)
            {
                for (int i = 0; i < 7; ++i)
                {
                    if (i != 0)
                    {
                        discardPile.Push(new Card(color, Convert.ToString(i))); // we always shuffle discard pile, we'll move to newDeck later
                    }
                    discardPile.Push(new Card(color, Convert.ToString(i)));
                }
            }
            // Shuffle(); will shuffle later in GetTopCard
        }

        public override Card GetCardFromDeck()
        {
            if (newDeck.Count == 0)
            {
                newDeck = discardPile;
                discardPile = new Stack<Card>();
            }
            return newDeck.Pop();
        }
        public Stack<Card> GetDiscardPile()
        {
            return discardPile;
        }

    }

    [TestClass]
    public class DeckTests
    {
        private Player player1;
        private Player player2;
        private TestDeck deck;


        [TestInitialize]
        public void Initialize()
        {
            deck = new TestDeck();
            player1 = new Player("Player1");
            player2 = new Player("Player2");

        }

        [TestMethod]
        public void GetCardFromDeckTest()
        {
            for (int i = 0; i < 5; i++)
            {
                player1.DrawCard(deck.GetCardFromDeck()); // yellow 6, yellow 6, yellow 5, yellow 5, yellow 4
            }

            for (int i = 0; i < 5; i++)
            {
                player2.DrawCard(deck.GetCardFromDeck()); // yellow 4, yellow 3, yellow 3, yellow 2, yelllow 2
            }
            List<Card> playerOneCards = new List<Card>
                {
                    new Card(ConsoleColor.DarkYellow, "6"),
                    new Card(ConsoleColor.DarkYellow, "6"),
                    new Card(ConsoleColor.DarkYellow, "5"),
                    new Card(ConsoleColor.DarkYellow, "5"),
                    new Card(ConsoleColor.DarkYellow, "4")
                };
            List<Card> playerTwoCards = new List<Card>
                {
                    new Card(ConsoleColor.DarkYellow, "4"),
                    new Card(ConsoleColor.DarkYellow, "3"),
                    new Card(ConsoleColor.DarkYellow, "3"),
                    new Card(ConsoleColor.DarkYellow, "2"),
                    new Card(ConsoleColor.DarkYellow, "2")
                };

            CollectionAssert.AreEqual(player1.GetHand(), playerOneCards);
            CollectionAssert.AreEqual(player2.GetHand(), playerTwoCards);

        }

        [TestMethod]
        public void DeckTest()
        {
            //Assert that deck has 46 cards and create a loop to match those cards
            Stack<Card> deckCards = new Stack<Card>();
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.DarkYellow };
            foreach (ConsoleColor color in colors)
            {
                for (int i = 0; i < 7; ++i)
                {
                    if (i != 0)
                    {
                        deckCards.Push(new Card(color, Convert.ToString(i))); // we always shuffle discard pile, we'll move to newDeck later
                    }
                    deckCards.Push(new Card(color, Convert.ToString(i)));
                }
            }
            CollectionAssert.AreEqual(deck.GetDiscardPile(), deckCards);
        }

    }
}
