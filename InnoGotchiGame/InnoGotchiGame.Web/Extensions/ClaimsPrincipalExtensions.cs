using System.Security.Claims;

namespace InnoGotchiGame.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(nameof(TokenClaims.UserId))?.Value;
        }
    }
}
