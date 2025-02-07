namespace Uno.tests
{
    [TestClass]
    public class CardTests
    {


        [TestMethod]
        public void CardEqualityTest()
        {
            Card card1 = new Card(ConsoleColor.Red, "5");
            Card card2 = new Card(ConsoleColor.Red, "5");

            Assert.IsTrue(card1.Equals(card2));
        }

        [TestMethod]
        public void AddCard_ShouldAddCardToHand()
        {
            // Arrange
            var hand = new Hand();
            var card = new Card(ConsoleColor.Red, "5");

            // Act
            hand.AddCard(card);

            // Assert
            CollectionAssert.Contains(hand.Cards, card);
        }
    }
}
