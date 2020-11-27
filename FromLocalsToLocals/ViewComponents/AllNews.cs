using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using FromLocalsToLocals.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class AllNews : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AllNews(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(PostVM model)
        {

            model.Posts = await _context.Posts.OrderByDescending(x => x).Include(x => x.Vendor).Join(_context.Vendors,
                                                post => post.VendorID,
                                                vendor => vendor.ID,
                                                (post,vendor) => Tuple.Create(post,vendor.Image)).ToListAsync();

            return View(model);
        }
    }
}
