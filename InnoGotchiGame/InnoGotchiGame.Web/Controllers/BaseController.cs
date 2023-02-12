﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BaseController : Controller
    {
    }
}
