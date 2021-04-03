using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Services.EF
{
    public interface IPostsService
    {
        Task<IActionResult> GetAllPostsAsync(int skip, int take);
        Task<IActionResult> GetVendorPostsAsync(int vendorId, int skip, int take);
        Task<IActionResult> GetFollowingPostsAsync(string userId, int skip, int take);
        Task CreatePost(string text, Vendor vendor, IFormFile image);
    }
}