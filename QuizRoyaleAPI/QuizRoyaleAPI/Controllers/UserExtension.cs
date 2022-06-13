using System.Security.Claims;

namespace QuizRoyaleAPI.Controllers
{
    /// <summary>
    /// UserExtension, geeft een user een GetID mehode
    /// </summary>
    public static class UserExtension
    {
        public static int GetID(this ClaimsPrincipal User)
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
