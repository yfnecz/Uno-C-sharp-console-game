using System.Text;

namespace Uno
{
    public class Hand
    {
        public List<Card> Cards { get; }

        public Hand(List<Card>? cards = null)
        {
            if (cards == null)
            {
                cards = new List<Card>();
            }
            Cards = cards;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public List<StringBuilder> ToStrings(int width, int height)
        {
            if (Cards.Count == 0)
            {
                return new List<StringBuilder>();
            }

            int gapWidth = 2;
            string gap = new string(' ', gapWidth);

            List<StringBuilder> rows = new();
            for (int i = 0; i < height + 1; i++)
            {
                rows.Add(new StringBuilder());
            }

            foreach (Card card in Cards)
            {
                List<string> cardRows = card.ToStrings(width, height);
                // + " " since the "_" along the top border of each card don't extend all the way to the right border
                rows[0].Append(cardRows[0] + " " + gap);
                for (int i = 1; i < cardRows.Count; i++)
                {
                    rows[i].Append(cardRows[i] + gap);
                }
            }

            return rows;
        }
    }
}