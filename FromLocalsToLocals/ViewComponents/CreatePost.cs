using FromLocalsToLocals.Models.Services;
using FromLocalsToLocals.Models.ViewModels;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class CreatePost : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CreatePostVM model)
        {
            return View(model);
        }
    }
}
