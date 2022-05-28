using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
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
            ));
        }

        public IEnumerable<QuestionDTO> GetQuestionByCategoryId(int categoryId)
        {
            return _context.Questions.Where(q => q.CategoryId == categoryId).Select(q => new QuestionDTO(
                q.Content,
                q.RightAnswer,
                q.Possibilities
            ));
        }
    }
}
