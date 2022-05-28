using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IPlayerDataService
    {
        public DivisionDTO GetDivision(int userId);

        public IEnumerable<ResultDTO> GetResults(int userId);

        public IEnumerable<MasteryDTO> GetMastery(int userId);

        public IEnumerable<BadgeDTO> GetBadges(int userId);
    }
}
