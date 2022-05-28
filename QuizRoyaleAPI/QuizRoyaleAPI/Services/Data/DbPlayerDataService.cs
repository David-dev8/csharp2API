using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public class DbPlayerDataService : IPlayerDataService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbPlayerDataService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public DivisionDTO GetDivision(int userId)
        {
            int position = GetPlayerPosition(userId);
            var division = _context.Ranks.Join(_context.Divisions,
                r => r.Id,
                d => d.RankId,
                (r, d) => new
                {
                    Division = d,
                    Rank = r
                }
            ).Where(d => d.Division.UpperBound > position).OrderBy(d => d.Division.UpperBound).Single();

            return new DivisionDTO(
                division.Rank.Name,
                division.Rank.Color,
                division.Division.Number,
                division.Division.Picture,
                division.Division.UpperBound
            );
        }

        public IEnumerable<MasteryDTO> GetMastery(int userId)
        {
            return GetPlayer(userId).Mastery.Join(_context.Categories,
                cm => cm.CategoryId,
                c => c.Id,
                (cm, c) => new
                {
                    Category = c,
                    SuccessRate = (cm.QuestionsRight / (double)cm.AmountOfQuestions)
                }
            ).Select(m => new MasteryDTO(
                new CategoryDTO(
                    m.Category.Name,
                    m.Category.Color,
                    m.Category.Picture),
                Math.Round(m.SuccessRate, 2)
            ));
        }

        public IEnumerable<ResultDTO> GetResults(int userId)
        {
            return GetPlayer(userId).Results.Select(r => new ResultDTO(
                r.Mode,
                r.RightAnswers,
                r.Position
            ));
        }

        private Player GetPlayer(int userId)
        {
            Player? player = _context.Players.Find(userId);
            if (player == null)
            {
                throw new PlayerNotFoundException();
            }
            return player;
        }

        private int GetPlayerPosition(int userId)
        {
            int position = _context.Players.OrderBy(p => p.AmountOfWins).ToList().FindIndex((p) => p.Id == userId);
            return (int)((double)100 * position / _context.Players.Count());
        }
    }
}
