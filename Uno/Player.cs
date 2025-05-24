using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Uno
{
    public class Player
    {
        public string name { get; internal set; }
        public Hand playerHand { get; set; }
        public bool saidUno { get; set; }

        public Player(string playerName)
        {
            name = playerName;
            playerHand = new Hand();
            saidUno = false;
        }

        public void DrawCard(Card card)
        {
            playerHand.AddCard(card);
            saidUno = false;
        }

        public Card PlayCard(int index)
        {
            var card = playerHand.Cards[index];
            playerHand.Cards.RemoveAt(index);
            return card;
        }

        public bool PlayerHandEmpty()
        {
            return playerHand.Cards.Count == 0;
        }

        public List<Card> GetHand()
        {
            return playerHand.Cards;
        }

        public bool hasUno()
        {
            return playerHand.Cards.Count == 1;
        }

        public void changeUno()
        {
            saidUno = !saidUno;
        }
    }
}