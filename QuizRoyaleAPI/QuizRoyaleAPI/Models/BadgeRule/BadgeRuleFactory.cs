﻿using QuizRoyaleAPI.Exceptions;

namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de factory voor BadgeRules
    /// </summary>
    public class BadgeRuleFactory
    {
        /// <summary>
        /// Maakt een BadgeRule aan de hand van het type
        /// </summary>
        /// <param name="type">De type badgeRule dat je wil maken</param>
        /// <param name="gradation">Het level van de badge</param>
        /// <returns>Een BadgeRule</returns>
        /// <exception cref="UnsupportedRuleException">Deze exceptie wordt gegooid zodra er geen badgeRule kan worden gevonden dat overeenkomt met de gegeven type</exception>
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
