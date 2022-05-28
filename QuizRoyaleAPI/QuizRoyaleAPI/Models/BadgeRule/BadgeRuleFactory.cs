using QuizRoyaleAPI.Exceptions;

namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class BadgeRuleFactory
    {
        public IBadgeRule GetRule(BadgeType type, int gradation)
        {
            // todo factory met enums of strings?
            return type switch
            {
                BadgeType.WINSTREAK => new WinStreakBadgeRule(),
                BadgeType.TOTAL_WINS => new TotalWinsBadgeRule(),
                BadgeType.ROYALE_PLAYED => new RoyaleGamesBadgeRule(),
                BadgeType.ITEM_UNLOCKER => new ItemBadgeRule(),
                BadgeType.MASTERY => new MasteryBadgeRule(),
                _ => throw new UnsupportedRuleException(),
            };
        }
    }
}
