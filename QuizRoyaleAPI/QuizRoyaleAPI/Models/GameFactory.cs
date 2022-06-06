using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Hubs;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Models
{
    public interface IGameFactory
    {
        public Game GetGame(int miliQuestionTime);
    }

    public class GameFactory
    {
        public static Game GetGame(int miliQuestionTime)
        {
            return new Game(miliQuestionTime);
        }
    }
}
