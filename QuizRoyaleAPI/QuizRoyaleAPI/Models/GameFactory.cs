using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Models
{
    public interface IGameFactory
    {
        public Game GetGame(int miliQuestionTime);
    }

    public class GameFactory : IGameFactory
    {
        private IQuestionService _QuestionService;
        private IPlayerService _PlayerService;

        public GameFactory(IQuestionService questionService, IPlayerService playerService) 
        {
            this._QuestionService = questionService;
            this._PlayerService = playerService;
        }

        public Game GetGame(int miliQuestionTime)
        {
            return new Game(miliQuestionTime, _QuestionService, _PlayerService);
        }
    }
}
