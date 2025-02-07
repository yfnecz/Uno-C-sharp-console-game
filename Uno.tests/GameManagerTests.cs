namespace Uno.tests
{
    [TestClass]
    public class GameManagerTests
    {
        [TestMethod]
        public void TestAskForPlayerNames_ValidInput()
        {
            // Arrange
            var gameManager = new GameManager();
            var input = new Queue<string>(new[] { "2", "Alice", "Bob" });
            Console.SetIn(new System.IO.StringReader(string.Join(Environment.NewLine, input)));

            // Act
            gameManager.AskForPlayerNames();

            // Assert
            Assert.AreEqual(2, gameManager.getPlayers().Count);
            Assert.AreEqual("Alice", gameManager.getPlayers()[0].name);
            Assert.AreEqual("Bob", gameManager.getPlayers()[1].name);
        }

        [TestMethod]
        public void TestDistributeCards()
        {
            // Arrange
            var gameManager = new GameManager();
            gameManager.getPlayers().Add(new Player("Alice"));
            gameManager.getPlayers().Add(new Player("Bob"));

            // Act
            gameManager.DistributeCards(gameManager.getPlayers());

            // Assert
            foreach (var player in gameManager.getPlayers())
            {
                Assert.AreEqual(7, player.GetHand().Count);
            }
        }

        [TestMethod]
        public void TestPlaySpecialCard_Skip()
        {
            // Arrange
            var gameManager = new GameManager();
            gameManager.getPlayers().Add(new Player("Alice"));
            gameManager.getPlayers().Add(new Player("Bob"));
            gameManager.currPlayerIdx = 0;
            var skipCard = new Card(ConsoleColor.Red, "X");

            // Act
            bool isSpecialCard = false;
            bool skippedTurnTwice = false;
            gameManager.PlaySpecialCard(ref isSpecialCard, skipCard, gameManager.getPlayers()[0], ref skippedTurnTwice, 1);

            // Assert
            Assert.AreEqual(0, gameManager.currPlayerIdx);
        }

        [TestMethod]
        public void TestPlayerTurn()
        {
            // Arrange
            var gameManager = new GameManager();
            gameManager.getPlayers().Add(new Player("Alice"));
            gameManager.getPlayers().Add(new Player("Bob"));
            gameManager.currPlayerIdx = 0;
            gameManager.direction = 1;

            // Act
            var nextPlayer = gameManager.PlayerTurn();

            // Assert
            Assert.AreEqual("Bob", nextPlayer.name);
        }

        [TestMethod]
        public void TestDetermineIfWinner()
        {
            // Arrange
            var gameManager = new GameManager();
            var player = new Player("Alice");
            player.DrawCard(new Card(ConsoleColor.Red, "5"));

            // Act
            bool isWinner = gameManager.DetermineIfWinner(player);

            // Assert
            Assert.IsFalse(isWinner);

            // Simulate player having no cards
            player.playerHand = new Hand(new List<Card>());
            isWinner = gameManager.DetermineIfWinner(player);

            // Assert
            Assert.IsTrue(isWinner);
        }
    }
}
