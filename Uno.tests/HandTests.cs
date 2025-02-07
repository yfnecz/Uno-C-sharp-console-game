using System.ComponentModel;
using System.Drawing;
using Uno;

namespace Uno.tests
{
    [TestClass]
    public sealed class HandTests
    {

        [TestMethod]
        public void Hand_InitializationWithCards_ShouldSetCardsProperty()
        {
            // Arrange
            var cards = new List<Card>
            {
                new Card(ConsoleColor.Red, "5"),
                new Card(ConsoleColor.Blue, "7")
            };

            // Act
            var hand = new Hand(cards);

            // Assert
            CollectionAssert.AreEqual(cards, hand.Cards);
        }

        [TestMethod]
        public void Hand_InitializationWithNull_ShouldCreateEmptyHand()
        {
            // Arrange & Act
            var hand = new Hand(null);

            // Assert
            Assert.AreEqual(0, hand.Cards.Count);
        }

        [TestMethod]
        public void ToStrings_EmptyHand_ShouldReturnEmptyList()
        {
            // Arrange
            var hand = new Hand();

            // Act
            var result = hand.ToStrings(5, 5);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ToStrings_NonEmptyHand_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var hand = new Hand(new List<Card>
            {
                new Card(ConsoleColor.Red, "5"),
                new Card(ConsoleColor.Blue, "7")
            });

            // Act
            var result = hand.ToStrings(5, 5);

            // Assert
            Assert.AreEqual(6, result.Count); // height + 1
        }

    }

}
