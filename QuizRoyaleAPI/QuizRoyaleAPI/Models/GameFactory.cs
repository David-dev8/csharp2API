using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Hubs;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is de interface voor een game factory
    /// </summary>
    public interface IGameFactory
    {
        public Game GetGame(int miliQuestionTime);
    }

    /// <summary>
    /// Dit is een implementatie van de game factory interface
    /// </summary>
    public class GameFactory
    {
        /// <summary>
        /// Maakt een nieuwe game aan de hand van question time
        /// </summary>
        /// <param name="miliQuestionTime">De tijd die je hebt om vragen te beantwoorden in een game</param>
        /// <returns>Een Game</returns>
        public static Game GetGame(int miliQuestionTime)
        {
            return new Game(miliQuestionTime);
        }
    }
}
