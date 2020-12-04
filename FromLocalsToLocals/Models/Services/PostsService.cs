

using FromLocalsToLocals.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace FromLocalsToLocals.Models.Services
{
    public class PostsService : IPostsService
    {
        private readonly AppDbContext _context;

        public PostsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("Something unexpected happened while trying to add post");
            }

        }

        public async Task<IActionResult> GetAllPosts(int skip, int take)
        {
            try
            {
                return new JsonResult(await _context.Posts.OrderByDescending(x => x).Skip(skip).Take(take).ToListAsync());
            }
            catch
            {
                throw new Exception("Something unexpected happened while trying to load posts");
            }

        }

        public async Task<IActionResult> GetFollowingPosts(string userId, int skip, int take)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                var listOfPosts = new List<Post>();
                foreach(var follower in user.Following)
                {
                    listOfPosts.AddRange(follower.Vendor.Posts.OrderByDescending(x=>x).Skip(skip).Take(take));
                }
                return new JsonResult(listOfPosts);

            }
            catch
            {
                throw new Exception("Something unexpected happened while trying to load posts");
            }
        }

        public async Task<IActionResult> GetVendorPosts(int vendorId, int skip, int take)
        {
            try
            {
                return new  JsonResult(await _context.Posts.Where(x => x.VendorID == vendorId).OrderByDescending(x => x).Skip(skip).Take(take).ToListAsync());
            }
            catch
            {
                throw new Exception("Something unexpected happened while trying to load posts");
            }
        }
    }
}
