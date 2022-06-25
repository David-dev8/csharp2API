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
            Claim? idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(idClaim == null)
            {
                throw new ArgumentNullException();
            }
            return int.Parse(idClaim.Value);
        }
    }
}
