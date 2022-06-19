using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Models.BadgeRule;
using QuizRoyaleAPI.Enums;

namespace QuizRoyaleAPI.Services.Data.Database
{
    /// <summary>
    /// DbPlayerDataService, Een implementatie van de PlayerDataService die comuniceerd met een Database
    /// </summary>
    public class DbPlayerDataService : IPlayerDataService
    {
        private readonly QuizRoyaleDbContext _context;
        private readonly BadgeRuleFactory _badgeRuleFactory;

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
            ).Where(d => d.Division.UpperBound >= position).OrderBy(d => d.Division.UpperBound).First();

            return new DivisionDTO(
                division.Rank.Name,
                division.Rank.Color,
                division.Division.Number,
                division.Division.Picture,
                division.Division.UpperBound
            );
        }

        public IEnumerable<CategoryIntensityDTO> GetMastery(int userId)
        {
            return _context.Categories.ToList().GroupJoin(GetPlayer(userId).Mastery.ToList(),
                c => c.Id,
                cm => cm.CategoryId,
                (c, cm) => new
                {
                    Category = c,
                    Rates = cm
                }
            ).SelectMany(
                m => m.Rates.DefaultIfEmpty(),
                (c, cm) => new CategoryIntensityDTO(
                    new CategoryDTO(
                        c.Category.Id,
                        c.Category.Name,
                        c.Category.Color,
                        c.Category.Picture),
                    GetPercentageOfRightAnswers(cm))
            ).ToList();
        }

        private double GetPercentageOfRightAnswers(CategoryMastery? mastery)
        {
            if(mastery == null)
            {
                return 0;
            }
            else
            {
                return 100 * Math.Round(mastery.AmountOfQuestions == 0 ? 0 : mastery.QuestionsRight / (double)mastery.AmountOfQuestions, 2);
            }
        }

        public IEnumerable<ResultDTO> GetResults(int userId)
        {
            return GetPlayer(userId).Results.OrderByDescending(r => r.Time).Select(r => new ResultDTO(
                r.Mode,
                r.Time,
                r.Position
            ));
        }

        public IEnumerable<BadgeDTO> GetBadges(int userId)
        {
            Player player = GetPlayer(userId);

            IList<Badge> earnedBadges = player.Badges.ToList();
            IList<Badge> badges = _context.Badges.ToList();

            IList<BadgeDTO> allEarnedBadges = badges.Select(b => new BadgeDTO(
                b.Name,
                b.Picture,
                b.Description,
                earnedBadges.Contains(b) || Reward(b, player)
            )).ToList();
            _context.SaveChanges();
            return allEarnedBadges;
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
            if(GetPlayer(userId).AmountOfWins == 0)
            {
                return 100;
            } 
            else
            {
                int position = _context.Players.OrderByDescending(p => p.AmountOfWins).ToList().FindIndex((p) => p.Id == userId);
                return (int)((double)100 * position / _context.Players.Count());
            }
        }

        private bool Reward(Badge badge, Player player)
        {
            bool canBeRewarded = CanBeRewarded(badge, player);
            if(canBeRewarded)
            {
                player.Badges.Add(badge);
            }
            return canBeRewarded;
        }

        private bool CanBeRewarded(Badge badge, Player player)
        {
            return _badgeRuleFactory.GetRule(badge.Type).HasEarned(player, badge.Gradation);
        }
    }
}
