using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public interface IPlayerDataService
    {
        public Rank GetRank(int userId);

        public IEnumerable<Result> GetResults(int userId);

        public IDictionary<Category, float> GetMastery(int userId);
    }
}
