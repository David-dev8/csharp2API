using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IQuestionService
    {
        public IEnumerable<Category> GetCategories();

        public IEnumerable<Question> GetQuestions();

        public IEnumerable<Question> GetQuestionByCategoryId(int categoryId);
    }
}
