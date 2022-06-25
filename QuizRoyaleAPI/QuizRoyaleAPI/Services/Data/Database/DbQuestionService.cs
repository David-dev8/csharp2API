using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data.Database
{
    /// <summary>
    /// DbQuestionService, Een implementatie van de QuestionService die communiceert met een database
    /// </summary>
    public class DbQuestionService: IQuestionService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbQuestionService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            return _context.Categories.Select(c => new CategoryDTO(
                c.Id,
                c.Name,
                c.Color,
                c.Picture
            )).ToList();
        }

        public QuestionDTO GetQuestionByCategoryId(int categoryId)
        {
            int totalAmountOfQuestionsInCategory = _context.Questions.Where(q => q.CategoryId == categoryId).Count();
            int randomRow = new Random().Next(totalAmountOfQuestionsInCategory);

            Question randomQuestion = _context.Questions.Where(q => q.CategoryId == categoryId).Skip(randomRow).First();
            return new QuestionDTO(
                randomQuestion.Id,
                randomQuestion.Content,
                randomQuestion.RightAnswer,
                randomQuestion.Possibilities.Select(p => new AnswerDTO(
                    p.Code,
                    p.Description
                ))
            );
        }
    }
}
