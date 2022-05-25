namespace QuizRoyaleAPI.Models
{
    public class DivisionDTO
    {
        public string Rank { get; set; }

        public int Number { get; set; }

        public string Picture { get; set; }

        public double UpperBound { get; set; }

        public DivisionDTO(string rank, int number, string picture, double upperBound)
        {
            Rank = rank;
            Number = number;
            Picture = picture;
            UpperBound = upperBound;
        }
    }
}
