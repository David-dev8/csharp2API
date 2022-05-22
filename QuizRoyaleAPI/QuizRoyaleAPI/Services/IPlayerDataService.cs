using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public interface IPlayerDataService
    {
        public Rank getRank(string username);

        public IEnumerable<Result> getResults(string username);

        public IDictionary<Category, float> getMastery(string username);
    }
}
