using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Data
{
    /// <summary>
    /// De interface voor een question service
    /// Een question service regelt dingen met betrekking tot vragen
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Haalt alle categorieen op
        /// </summary>
        /// <returns>Een colectie met categorieen</returns>
        public IEnumerable<CategoryDTO> GetCategories();

        /// <summary>
        /// Haalt een vraag op voor een gegeven categorie
        /// </summary>
        /// <param name="categoryId">De ID van de categorie waarvan je een vraag wilt</param>
        /// <returns>Een QuestionDTO</returns>
        public QuestionDTO GetQuestionByCategoryId(int categoryId);
    }
}
