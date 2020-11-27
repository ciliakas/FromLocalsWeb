using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class LovedVendorsNews : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public LovedVendorsNews(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
