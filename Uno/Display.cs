﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Uno
{
    public static class Display
    {
        public static int DEFAULT_CARD_DISPLAY_WIDTH = 6;
        public static int DEFAULT_CARD_DISPLAY_HEIGHT = 5;
        private static ConsoleColor[] colors = new ConsoleColor[]
        {
                ConsoleColor.Red,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.Cyan,
                ConsoleColor.Blue
        };

        public static ConsoleColor SelectColor(List<Player> players, int curPlayerIndex, int direction)
        {
            int cursorPosition = Display.DisplaySelection(players, 0, curPlayerIndex, direction);
            ConsoleColor color = ConsoleColor.Black;
            switch (cursorPosition)
            {
                case 0:
                    color = ConsoleColor.Red;
                    break;
                case 1:
                    color = ConsoleColor.Green;
                    break;
                case 2:
                    color = ConsoleColor.Blue;
                    break;
                case 3:
                    color = ConsoleColor.DarkYellow;
                    break;
            }
            return color;
        }

        public static int DisplaySelection(List<Player> players, int cursorPosition, int curPlayerIndex, int direction)
        {
            Hand selectionHand = new Hand();
            selectionHand.AddCard(new Card(ConsoleColor.Red, "Red"));
            selectionHand.AddCard(new Card(ConsoleColor.Green, "Green"));
            selectionHand.AddCard(new Card(ConsoleColor.Blue, "Blue"));
            selectionHand.AddCard(new Card(ConsoleColor.DarkYellow, "Yellow"));

            while (true)
            {
                Console.Clear();
                Console.Write("Number of cards per player: ");
                foreach (Player p in players)
                {
                    Console.Write($"{p.name}: {p.GetHand().Count} ");
                }
                Console.WriteLine();
                string dir = direction == 1 ? "→" : "←";
                Console.WriteLine($"{players[curPlayerIndex].name}'s Turn {dir}");
                Console.WriteLine();
                Console.WriteLine("Don't forget to say 'Uno' at the end of your turn!");
                Console.WriteLine();
                Console.WriteLine("Your cards:");
                Display.DisplayPlayerHand(players[curPlayerIndex].playerHand, -1);
                Console.WriteLine();
                Console.WriteLine("Select a color");
                Display.DisplayPlayerHand(selectionHand, cursorPosition);
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (cursorPosition > 0)
                        {
                            cursorPosition--;
                        }
                        else
                        {
                            cursorPosition = selectionHand.Cards.Count - 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorPosition < selectionHand.Cards.Count - 1)
                        {
                            cursorPosition++;
                        }
                        else
                        {
                            cursorPosition = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return cursorPosition;
                }
            }
        }
        public static int selectOption(bool cardDrawn, List<Player> players, int curPlayerIndex, Card topCard, int direction) //, out bool saidUno
        {
            int selectedOption = -1;
            int cursorPosition = 0;
            List<Card> playerCards = players[curPlayerIndex].GetHand();
            int cardDisplayWidth = DEFAULT_CARD_DISPLAY_WIDTH + 2;
            int maxCardsPerRow = Math.Max(1, (Console.WindowWidth - 2) / (cardDisplayWidth + 2)); // 2 for gap

            while (selectedOption == -1)
            {
                DisplayBoard(players, curPlayerIndex, cursorPosition, topCard, direction);

                if (cardDrawn)
                {
                    Console.WriteLine("Use arrow keys to select a card to play or press Space to skip your turn.");
                }
                else
                {
                    Console.WriteLine("Use arrow keys to select a card to play or press Space to draw a new card.");
                }

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (cursorPosition > 0)
                        {
                            cursorPosition--;
                        }
                        else
                        {
                            cursorPosition = playerCards.Count - 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorPosition < playerCards.Count - 1)
                        {
                            cursorPosition++;
                        }
                        else
                        {
                            cursorPosition = 0;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (cursorPosition >= maxCardsPerRow)
                        {
                            cursorPosition -= maxCardsPerRow;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursorPosition + maxCardsPerRow < playerCards.Count)
                        {
                            cursorPosition += maxCardsPerRow;
                        }
                        break;
                    case ConsoleKey.Enter:
                        var playerCard = playerCards[cursorPosition];
                        if (playerCard.color != ConsoleColor.Black && topCard.color != ConsoleColor.Black)
                        {
                            if (playerCard.color != topCard.color && playerCard.FaceVal != topCard.FaceVal)
                            {
                                Console.WriteLine($"Invalid selection. Selected card does not match the open card");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else
                            {
                                selectedOption = cursorPosition + 1;
                            }
                        }
                        else
                        {
                            if (topCard.color == ConsoleColor.Black && playerCard.color != ConsoleColor.Black && playerCard.color != topCard.selectedColor)
                            {
                                Console.WriteLine($"Invalid selection. Selected card does not match the open card");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                            else
                            {
                                selectedOption = cursorPosition + 1;
                            }
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        selectedOption = 0;
                        break;

                }
            }

            return selectedOption;
        }
        public static void DisplayCard(Card card)
        {
            int width = DEFAULT_CARD_DISPLAY_WIDTH;
            int height = DEFAULT_CARD_DISPLAY_HEIGHT;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (card.color != ConsoleColor.Black)
            {
                Console.ForegroundColor = card.color;
                Console.WriteLine(String.Join("\n", card.ToStrings(width, height)));
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = card.selectedColor;
                Console.WriteLine(String.Join("\n", card.ToStrings(width, height)));
                Console.ResetColor();
            }
        }

        public static void DisplayBoard(List<Player> players, int curPlayerIndex, int cursorPosition, Card topCard, int direction)
        {
            Console.Clear();
            Console.Write("Number of cards per player: ");
            foreach (Player p in players)
            {
                Console.Write($"{p.name}: {p.GetHand().Count} ");
            }
            Console.WriteLine();
            Console.WriteLine("Don't forget to say 'Uno' at the end of your turn!");
            Console.WriteLine();
            string dir = direction == 1 ? "→" : "←";
            Console.WriteLine($"{players[curPlayerIndex].name}'s Turn {dir}");
            Console.WriteLine();
            Console.WriteLine("Current Card:");
            DisplayCard(topCard);

            Console.WriteLine();
            Console.WriteLine($"{players[curPlayerIndex].name}'s Cards");
            Console.WriteLine();
            DisplayPlayerHand(players[curPlayerIndex].playerHand, cursorPosition);
            Console.WriteLine();
        }

        public static void DisplayPlayerHand(Hand playerHand, int cursorPosition)
        {
            int width = DEFAULT_CARD_DISPLAY_WIDTH;
            int height = DEFAULT_CARD_DISPLAY_HEIGHT;
            int cardDisplayWidth = width + 2; // adjust if needed for borders/gaps
            int maxCardsPerRow = Math.Max(1, (Console.WindowWidth - 2) / (cardDisplayWidth + 2)); // 2 for gap

            int totalCards = playerHand.Cards.Count;
            int numRows = (int)Math.Ceiling((double)totalCards / maxCardsPerRow);

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine();

            for (int row = 0; row < numRows; row++)
            {
                int start = row * maxCardsPerRow;
                int count = Math.Min(maxCardsPerRow, totalCards - start);
                if (count <= 0) continue;

                var subHand = new Hand(playerHand.Cards.GetRange(start, count));
                List<StringBuilder> rows = subHand.ToStrings(width, height);

                for (int j = 0; j < rows.Count; j++)
                {
                    StringBuilder rowStr = rows[j];
                    int card_length = rowStr.Length / subHand.Cards.Count;
                    for (int i = 0; i < subHand.Cards.Count; i++)
                    {
                        int globalIndex = start + i;
                        if (globalIndex == cursorPosition)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        if (playerHand.Cards[globalIndex].color != ConsoleColor.Black)
                        {
                            Console.ForegroundColor = playerHand.Cards[globalIndex].color;
                        }
                        else
                        {
                            Console.ForegroundColor = colors[j % colors.Length];
                        }
                        Console.Write(rowStr.ToString().Substring(i * card_length, card_length - 2));
                        Console.ResetColor();
                        Console.Write("  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        public static void printRainbowMessage(string[] message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Console.ForegroundColor = colors[i % colors.Length];
                Console.WriteLine(message[i]);
            }

            Console.ResetColor();
        }

        public static void DisplayWinner(Player player)
        {
            Console.Clear();
            string[] winnerMessage = new string[]
            {
                @" __        _____ _   _ _   _ _____ ____  ",
                @" \ \      / /_ _| \ | | \ | | ____|  _ \ ",
                @"  \ \ /\ / / | ||  \| |  \| |  _| | |_) |",
                @"   \ V  V /  | || |\  | |\  | |___|  _ < ",
                @"    \_/\_/  |___|_| \_|_| \_|_____|_| \_\"
            };
            printRainbowMessage(winnerMessage);
            Console.WriteLine();
            Console.WriteLine($"{player.name} has won the game!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit the game...");
            Console.ReadKey();
        }

        public static void PrintGreeting()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;

            string[] welcomeMessage = new string[]
            {
        @" __        __   _                            _          _   _ ",
        @" \ \      / /__| | ___ ___  _ __ ___   ___  | |_ ___   | | | | _ __   ___ ",
        @"  \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \ | __/ _ \  | | | || '_ \ / _ \",
        @"   \ V  V /  __/ | (_| (_) | | | | | |  __/ | || (_) | | |_| || | | | (_) |",
        @"    \_/\_/ \___|_|\___\___/|_| |_| |_|\___|  \__\___/   \___/ |_| |_|\___/",
            };

            Display.printRainbowMessage(welcomeMessage);
            Console.WriteLine();
            Console.WriteLine("Press any key to start the game...");
            Console.ReadKey();
        }

        public static void DisplayUno()
        {
            string[] unoMessage = new string[]
            {
             @" _    _ _   _  ____",
             @"| |  | | \ | |/ __ \ ",
             @"| |  | |  \| | |  | |",
             @"| |  | | . ` | |  | |",
             @"| |__| | |\  | |__| |",
             @" \____/|_| \_|\____/",
            };
            printRainbowMessage(unoMessage);
            Console.WriteLine();
        }
    }
}
