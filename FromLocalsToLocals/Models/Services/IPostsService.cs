using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.Services
{
    public interface IPostsService
    {
        Task<IActionResult> GetAllPosts(int skip, int take);

        [HttpGet]
        Task<IActionResult> GetVendorPosts(int vendorId,int skip, int take);
        Task<IActionResult> GetFollowingPosts(string userId,int skip, int take);
        Task CreatePost(Post post);
    }
}
