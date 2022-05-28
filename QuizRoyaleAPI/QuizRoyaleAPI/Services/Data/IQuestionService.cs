using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IQuestionService
    {
        public IEnumerable<CategoryDTO> GetCategories();

        public IEnumerable<QuestionDTO> GetQuestionByCategoryId(int categoryId);
    }
}
