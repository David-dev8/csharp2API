namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de interface voor de badgeRules.
    /// </summary>
    public interface IBadgeRule
    {
        /// <summary>
        /// Kijkt of een speler deze badge heeft verdient.
        /// </summary>
        /// <param name="player">De speler die gecheckt moet worden.</param>
        /// <param name="gradation">De gradatie waartegen gecontroleert moet worden. Hoe hoger de gradatie, hoe moeilijker een badge te verkrijgen is.</param>
        /// <returns>True als de speler deze badge verdient, anders false.</returns>
        public bool HasEarned(Player player, int gradation);
    }
}
