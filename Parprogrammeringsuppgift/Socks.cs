using System.Text.Json;

namespace Parprogrammeringsuppgift
{
    internal class Sock
    {
        public int Size { get; set; }

        public string Color { get; set; } = "";

        private int _rating;

        public int Rating
        {
            get { return _rating; }
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("Betyget måste vara mellan 1 till 5");
                }
                _rating = value;
            }
        }

        public  Sock(int size, string color, int rating)
        {
            Size = size;
            Color = color;
            Rating = rating;
        }
    }
}
