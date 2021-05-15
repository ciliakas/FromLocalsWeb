using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.Controllers
{
    public class SwipeCardController : Controller
    {
        public IActionResult SwipeCard()
        {
            return View();
        }
    }
}
