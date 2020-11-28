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
    public class NewsFeed : ViewComponent
    {
        private readonly AppDbContext _context;

        public NewsFeed(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(PostVM model)
        {
            switch (model.ActiveTab)
            {
                case Tab.MyFeed:
                    model.Posts = await _context.Posts.OrderBy(x => x).Include(x => x.Vendor).Join(_context.Vendors,
                                               post => post.VendorID,
                                               vendor => vendor.ID,
                                               (post, vendor) => Tuple.Create(post, vendor.Image)).ToListAsync();
                    break;
                default:
                    model.Posts = await _context.Posts.OrderByDescending(x => x).Include(x => x.Vendor).Join(_context.Vendors,
                                               post => post.VendorID,
                                               vendor => vendor.ID,
                                               (post, vendor) => Tuple.Create(post, vendor.Image)).ToListAsync();
                    break;
            }
           

            return View(model);
        }
    }
}
