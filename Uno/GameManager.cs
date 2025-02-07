using Uno;
using System.Data;
using System.Numerics;
using System.Text;
using System.Drawing;

namespace Uno
{
    public class GameManager
    {
        public static int MIN_NUM_PLAYERS = 2;
        public static int MAX_NUM_PLAYERS = 10;
        Deck deck = new Deck();
        //Deck deck = new TestDeck();
        List<Player> players = new List<Player>();
        int numPlayers;
        public int currPlayerIdx { get; set; } = 0;
        public int direction { get; set; } = 1;
        Player player { get; set; } = new Player("Player");

        Player previousPlayer { get; set; }
        bool skippedTurnTwice;

        public List<Player> getPlayers()
        {
            return players;
        }

        public void AskForPlayerNames()
        {
            string reply;
            Console.WriteLine();
            do
            {
                Console.Write($"How many players (between {MIN_NUM_PLAYERS} and {MAX_NUM_PLAYERS} players)?   ");
                reply = Console.ReadLine().Trim();
            }
            while (!int.TryParse(reply, out numPlayers) || numPlayers < MIN_NUM_PLAYERS || numPlayers > MAX_NUM_PLAYERS);


            string name;
            for (int i = 1; i <= numPlayers; i++)
            {
                Console.WriteLine();
                do
                {
                    Console.Write($"What is player {i}'s name? (must be non-empty)    ");
                    name = Console.ReadLine().Trim();
                }
                while (String.IsNullOrEmpty(name));
                players.Add(new Player(name));
            }
            player = players[currPlayerIdx];
        }

        public void Play()
        {
            Display.PrintGreeting();
            AskForPlayerNames();
            DistributeCards(players);
            var winner = false;
            deck.addToDiscardPile(deck.GetCardFromDeck()); // add first open card on the table
            Card topCard = deck.viewTopCard();
            List<string> specialCards = new List<string> { "X", "⮏", "+2", "W", "+4" };
            while (topCard.color == ConsoleColor.Black || specialCards.Contains(topCard.FaceVal))
            {
                deck.addToDiscardPile(deck.GetCardFromDeck());
                topCard = deck.viewTopCard();
            }

            bool gameover = false;
            while (!gameover)
            {
                previousPlayer = players[currPlayerIdx];
                bool isSpecialCard = false;
                bool playerSaidUno = false;
                skippedTurnTwice = false;
                // check the top card to see if its a skip, reverse, or draw 2
                if (gameover == true)
                {
                    break;
                }
                //if (!player.hasUno() && player.saidUno)
                //{
                //    player.changeUno();
                //}
                int selectedOption = Display.selectOption(false, players, currPlayerIdx, deck.viewTopCard(), direction); //, out playerSaidUno
                //bool cardDrawn, List<Player> players, int curPlayerIndex, Card topCard
                if (selectedOption > 0)
                {
                    Card card = player.PlayCard(selectedOption - 1);
                    deck.addToDiscardPile(card);
                    winner = DetermineIfWinner(player);
                    if (!winner)
                    {
                        player = PlaySpecialCard(ref isSpecialCard, card, player, ref skippedTurnTwice, direction);
                    }
                }
                else
                {
                    //add a card to players hand and remove from new deck
                    player.DrawCard(deck.GetCardFromDeck());
                    //we show them selection options

                    selectedOption = Display.selectOption(true, players, currPlayerIdx, deck.viewTopCard(), direction); //, out playerSaidUno  card already drawn
                    //bool cardDrawn, List<Player> players, int curPlayerIndex, Card topCard
                    if (selectedOption > 0)
                    {
                        Card card = player.PlayCard(selectedOption - 1);
                        deck.addToDiscardPile(card);
                        winner = DetermineIfWinner(player);
                        if (!winner)
                        {
                            player = PlaySpecialCard(ref isSpecialCard, card, player, ref skippedTurnTwice, direction);
                        }
                    }
                    else
                    {
                        // player needs to skip turn
                        Console.WriteLine($"Player {player.name} skips turn");
                        player = PlayerTurn();

                    }

                }

                winner = DetermineIfWinner(player); // TODO: determine winner for every player
                if (!winner)
                {
                    // Player next_player = players[(currPlayerIdx + 1) % numPlayers];
                    Console.WriteLine($"Turn goes to the next player: {player.name}");
                    Console.WriteLine("Press enter to continue...");
                    //Console.ReadKey();
                    var input = Console.ReadLine()?.ToLower();
                    if (input != null && input == "uno")
                    {
                        UnoCalls();
                    }

                }
                else
                {
                    Display.DisplayWinner(player);
                    gameover = true;

                }
            }
        }

        private void UnoCalls()
        {
            bool validUnoCall = false;
            Console.Clear();
            Console.WriteLine($"Player {previousPlayer.name} has called Uno!");
            if (previousPlayer.hasUno())
            {
                previousPlayer.changeUno();
                validUnoCall = true;
            }
            var playersWithUno = players.Where(p => p.hasUno() && !p.saidUno);
            foreach (Player play in playersWithUno)
            {
                Console.WriteLine($"Player {play.name} did not call Uno and must draw 2 cards!");
                play.DrawCard(deck.GetCardFromDeck());
                play.DrawCard(deck.GetCardFromDeck());
                validUnoCall = true;

            }

            if (!validUnoCall)
            {
                Console.WriteLine("False Uno call!");
            } else
            {
                Display.DisplayUno();
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }



        public Player PlaySpecialCard(ref bool isSpecialCard, Card card, Player player, ref bool skippedTurnTwice, int direction)
        {
            if (card.FaceVal == "X" || card.FaceVal == "⮏" || card.FaceVal == "+2")
            {
                isSpecialCard = true;
                DoSpecialMove(card, ref isSpecialCard);
            }
            else if (card.color == ConsoleColor.Black)
            {
                card.setSelectedColor(Display.SelectColor(players, currPlayerIdx, direction));
                if (card.FaceVal == "+4")
                {
                    isSpecialCard = true;
                    DoSpecialMove(card, ref isSpecialCard);
                }
            }
            player = PlayerTurn();
            skippedTurnTwice = true;
            return player;
        }

        private void DoSpecialMove(Card card, ref bool isSpecialCard)
        {
            AdjustPreviousPlayer();
            switch (card.FaceVal)
            {
                case "X":
                    Player playerSkipped = PlayerTurn();
                    Console.WriteLine($"Player {playerSkipped.name} is skipped");
                    break;
                case "⮏":
                    if (players.Count == 2)
                    {
                        PlayerTurn();
                    }
                    direction = -direction;
                    Console.WriteLine($"The direction was reversed");
                    break;
                case "+2":
                    Player player = PlayerTurn();
                    player.DrawCard(deck.GetCardFromDeck());
                    player.DrawCard(deck.GetCardFromDeck());
                    Console.WriteLine($"Player {player.name} draws 2 cards and skips turn");
                    break;
                case "+4":
                    Player player4 = PlayerTurn();
                    for (int i = 0; i < 4; i++)
                    {
                        player4.DrawCard(deck.GetCardFromDeck());
                    }
                    Console.WriteLine($"Player {player4.name} draws 4 cards and skips turn");
                    break;
            }

            isSpecialCard = false;
        }

        public Player PlayerTurn()
        {
            
            if (currPlayerIdx == players.Count - 1 && direction == 1)
            {
                currPlayerIdx = 0;
            }
            else if (currPlayerIdx == 0 && direction == -1)
            {
                currPlayerIdx = players.Count - 1;
            }
            else
            {
                currPlayerIdx += direction;
            }
            return players[currPlayerIdx];
        }

        private void AdjustPreviousPlayer()
        {
            //if (skippedTurnTwice)
            //{
            //    var index = currPlayerIdx + direction;
            //    if (index < 0)
            //    {
            //        index = players.Count - 1;
            //    }
            //    else if (index == players.Count)
            //    {
            //        index = 0;
            //    }
            //    previousPlayer = players[index];
            //}
            //else
            //{
                previousPlayer = players[currPlayerIdx];
            //}
        }

        public bool DetermineIfWinner(Player player)
        {
            return player.PlayerHandEmpty();
        }

        public void DistributeCards(List<Player> players)
        {
            for (int i = 0; i < 7; i++)
            {
                foreach (Player player in players)
                {
                    player.DrawCard(deck.GetCardFromDeck());
                }
            }
            //players[0].DrawCard(new Card(ConsoleColor.Black, "W"));// just testing displaying wildcards
            //players[0].DrawCard(new Card(ConsoleColor.Black, "+4"));
        }

    }
}