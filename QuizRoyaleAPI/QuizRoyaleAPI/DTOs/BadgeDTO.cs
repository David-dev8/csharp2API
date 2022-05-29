namespace QuizRoyaleAPI.DTOs
{
    public class BadgeDTO
    {
        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public bool IsEarned { get; set; }

        public BadgeDTO(string name, string picture, string description, bool isEarned)
        {
            Name = name;
            Picture = picture;
            Description = description;
            IsEarned = isEarned;
        }
    }
}
