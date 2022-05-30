namespace QuizRoyaleAPI.DTOs
{
    public class DivisionDTO
    {
        public string Rank { get; set; }

        public string Color { get; set; }

        public int Number { get; set; }

        public string Picture { get; set; }

        public double UpperBound { get; set; }

        public DivisionDTO(string rank, string color, int number, string picture, double upperBound)
        {
            Rank = rank;
            Color = color;
            Number = number;
            Picture = picture;
            UpperBound = upperBound;
        }
    }
}
