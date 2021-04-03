using System.Threading.Tasks;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.ViewComponents
{
    public class CreatePost : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CreatePostVM model)
        {
            return View(model);
        }
    }
}