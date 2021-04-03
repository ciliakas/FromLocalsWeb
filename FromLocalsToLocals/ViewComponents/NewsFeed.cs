using System.Collections.Generic;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Utilities.Enums;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.ViewComponents
{
    public class NewsFeed : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IPostsService _postsService;

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
                    model.Posts =
                        new List<Post>(); // await _context.Posts.Where(x => x.VendorID == model.Vendor.ID).OrderByDescending(x => x).ToListAsync();
                    break;
                default: //AllFeed
                    model.Posts =
                        new List<Post>(); // await _context.Posts.Take(5).OrderByDescending(x => x).ToListAsync();
                    break;
            }

            return View(model);
        }
    }
}