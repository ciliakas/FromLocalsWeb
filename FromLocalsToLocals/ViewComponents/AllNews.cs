using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class AllNews : ViewComponent
    {

        public AllNews()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
