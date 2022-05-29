using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IQuestionService
    {
        public IEnumerable<CategoryDTO> GetCategories();

        public QuestionDTO GetQuestionByCategoryId(int categoryId);
    }
}
