using System.Security.Claims;

namespace QuizRoyaleAPI.Controllers
{
    public static class UserExtension
    {
        public static int GetID(this ClaimsPrincipal User)
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
