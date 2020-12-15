using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public interface IPostsService
    {
        Task<IActionResult> GetAllPostsAsync(int skip, int take);
        Task<IActionResult> GetVendorPostsAsync(int vendorId,int skip, int take);
        Task<IActionResult> GetFollowingPostsAsync(string userId,int skip, int take);
    }
}
