using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Utilities.Enums;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FromLocalsToLocals.Services.EF;

namespace FromLocalsToLocals.Web.ViewComponents
{
    public class NewsFeed : ViewComponent
    {
        private readonly IPostsService _postsService;
        private readonly AppDbContext _context;

        public NewsFeed(IPostsService postsService, AppDbContext context)
        {
            _postsService = postsService;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(FeedVM model)
        {
            switch (model.ActiveTab)
            {
                case FeedTabs.MyFeed:
                    model.Posts = new List<Post>();
                    break;
                case FeedTabs.VendorFeed:
                     model.Posts = new List<Post>();// await _context.Posts.Where(x => x.VendorID == model.Vendor.ID).OrderByDescending(x => x).ToListAsync();
                    break;
                default: //AllFeed
                     model.Posts = new List<Post>();// await _context.Posts.Take(5).OrderByDescending(x => x).ToListAsync();
                    break;
            }
           
            return View(model);
        }
    }
}
