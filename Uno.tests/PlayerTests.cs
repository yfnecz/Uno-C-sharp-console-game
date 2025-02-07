namespace Uno.tests
{
    [TestClass]
    public class PlayerTests
    {
        private Player player1 = new Player("one");


        [TestMethod]
        public void PlayCardTest()
        {
            Card card = new Card(ConsoleColor.Red, "5");
            player1.DrawCard(card);

            Card playedCard = player1.PlayCard(0);

            Assert.AreEqual(card, playedCard);
            Assert.IsTrue(player1.GetHand().Count == 0);
        }

        [TestMethod]
        public void DrawCardTest()
        {
            Card card = new Card(ConsoleColor.Red, "5");
            player1.DrawCard(card);

            List<Card> expectedHand = new List<Card> { card };
            CollectionAssert.AreEqual(expectedHand, player1.GetHand());
        }

        [TestMethod]
        public void PlayerHandEmptyTest()
        {
            Assert.IsTrue(player1.PlayerHandEmpty());

            Card card = new Card(ConsoleColor.Red, "5");
            player1.DrawCard(card);

            Assert.IsFalse(player1.PlayerHandEmpty());
        }

        [TestMethod]
        public void GetHand_ShouldReturnListOfCardsInHand()
        {
            // Arrange
            Player player = new Player("TestPlayer");
            Card card1 = new Card(ConsoleColor.Red, "5");
            Card card2 = new Card(ConsoleColor.Blue, "7");
            player.DrawCard(card1);
            player.DrawCard(card2);

            // Act
            List<Card> hand = player.GetHand();

            // Assert
            Assert.AreEqual(2, hand.Count);
            CollectionAssert.Contains(hand, card1);
            CollectionAssert.Contains(hand, card2);
        }

    }
}
