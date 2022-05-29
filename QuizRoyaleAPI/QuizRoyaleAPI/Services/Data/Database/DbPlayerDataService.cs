using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Models.BadgeRule;

namespace QuizRoyaleAPI.Services.Data.Database
{
    public class DbPlayerDataService : IPlayerDataService
    {
        private readonly QuizRoyaleDbContext _context;
        private readonly BadgeRuleFactory _badgeRuleFactory; // todo dependency injection? addScoped voor GameFactory?

        public DbPlayerDataService(QuizRoyaleDbContext context)
        {
            _context = context;
            _badgeRuleFactory = new BadgeRuleFactory();
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
                    SuccessRate = cm.AmountOfQuestions == 0 ? 0 : cm.QuestionsRight / (double)cm.AmountOfQuestions
                }
            ).Select(m => new MasteryDTO(
                new CategoryDTO(
                    m.Category.Id,
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

        public IEnumerable<BadgeDTO> GetBadges(int userId)
        {
            Player player = GetPlayer(userId);

            IList<Badge> earnedBadges = player.Badges.ToList();
            IList<Badge> badges = _context.Badges.ToList();

            return badges.Select(b => new BadgeDTO(
                b.Name,
                b.Picture,
                b.Description,
                earnedBadges.Contains(b) || CanBeRewarded(b, player)
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

        private bool CanBeRewarded(Badge badge, Player player)
        {
            return _badgeRuleFactory.GetRule(badge.Type, badge.Gradation).HasEarned(player);
        }
    }
}
