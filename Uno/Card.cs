using System.Drawing;
using System.Text;

namespace Uno
{
    public class Card
    {
        internal ConsoleColor color { get; }
        internal string FaceVal { get; }
        public ConsoleColor selectedColor { get; set; }
        public Card(ConsoleColor color, string faceVal)
        {
            this.color = color;
            FaceVal = faceVal;
            selectedColor = ConsoleColor.White;
        }

        public void setSelectedColor(ConsoleColor color)
        {
            selectedColor = color;
        }

        // width should be even and height should be odd so that the 2-char-wide suitString is centered
        // witdh and height should be at least 3 and 4 respectively
        // width is the number of '_' in the top border
        // height is the number of '|' in ea. of the left and right border
        public virtual List<string> ToStrings(int width, int height)
        {
            string v = "|";
            string f = FaceVal;

            List<string> rows = new();

            string topBorder = " " + new string('_', width);
            rows.Add(topBorder); // -1th row

            string topRow = v + f + new string(' ', width - f.Length) + v; // 0th row
            rows.Add(topRow);

            string otherRows = v + new string(' ', width) + v; // 1th through {height-2}th rows, except {height/2}th row
            for (int i = 1; i < height - 1; i++)
            {
                rows.Add(otherRows);
            }

            string bottomRow = v + new string('_', width - f.Length) + f + v; // {height-1}th row
            rows.Add(bottomRow);

            return rows;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Card otherCard)
            {
                return color == otherCard.color && FaceVal == otherCard.FaceVal;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(color, FaceVal);
        }
    }
}
