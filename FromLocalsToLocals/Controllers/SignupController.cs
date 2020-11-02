using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Controllers
{
    public class SignupController : Controller
    {

        public SignupController()
        {
        }



        public IActionResult Signup()
        {
            return View();
        }
    }
}
