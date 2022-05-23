using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public class DbQuestionService: IQuestionService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbQuestionService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Question> GetQuestions()
        {
            return _context.Questions.ToList();
        }

        public IEnumerable<Question> GetQuestionByCategoryId(int categoryId)
        {
            return _context.Categories.Find(categoryId).Questions;
        }
    }
}
