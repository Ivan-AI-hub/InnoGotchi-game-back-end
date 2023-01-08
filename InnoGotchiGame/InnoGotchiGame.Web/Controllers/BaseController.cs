using InnoGotchiGame.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api")]
    [Authorize("Bearer")]
    public class BaseController : Controller
    {
        protected int GetAuthUserId()
        {
            return int.Parse((User.FindFirst(nameof(TokenClaims.UserId))!.Value));
        }
    }
}
