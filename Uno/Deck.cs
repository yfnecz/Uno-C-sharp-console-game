using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace Uno
{
    public class Deck
    {
        protected Stack<Card> newDeck;
        protected Stack<Card> discardPile;
        public Card viewTopCard()
        {
            return discardPile.Peek();
        }

        public Deck()
        {
            newDeck = new Stack<Card>();
            discardPile = new Stack<Card>();
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.DarkYellow };

            foreach (ConsoleColor color in colors)
            {
                AddSpecialCard(color);
                for (int i = 0; i < 10; ++i)
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

        private void AddSpecialCard(ConsoleColor color)
        {
            string[] specialCards = { "X", "⮏", "+2"}; 
            foreach (string specialCard in specialCards)
            {
                discardPile.Push(new Card(color, specialCard));
                discardPile.Push(new Card(color, specialCard));
            }
            for (int i = 0; i < 4; ++i) {
                discardPile.Push(new Card(ConsoleColor.Black, "W"));
                discardPile.Push(new Card(ConsoleColor.Black, "+4"));
            }
        }

        public void Shuffle()
        {
            var rng = new Random();
            var values = discardPile.ToArray();
            discardPile.Clear();

            for (int n = values.Length - 1; n > 0; n--)
            {
                int k = rng.Next(n + 1);
                (values[k], values[n]) = (values[n], values[k]);
            }

            foreach (var value in values)
            {
                discardPile.Push(value);
            }
        }
        public virtual Card GetCardFromDeck()
        {
            if (newDeck.Count == 0)
            {
                Shuffle();
                newDeck = discardPile;
                discardPile = new Stack<Card>();
            }
            return newDeck.Pop();
        }

        public void addToDiscardPile(Card card) // we will add top card to the deck that everyone sees first turn
        {
            discardPile.Push(card);
        }

    }
}