using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Services.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FromLocalsToLocals.Web.Controllers
{
    [Authorize]
    public class FollowerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IFollowerService _followerService;

        public FollowerController(UserManager<AppUser> userManager, IFollowerService followerService)
        {
            _userManager = userManager;
            _followerService = followerService;
        }

        
        [HttpPost]
        [Route("/Follower/Follow/{id:int}")]
        [Route("/Follower/Sekti/{id:int}")]
        public async Task<IActionResult> Follow(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!await _followerService.Follow(user, id))
            {
                return BadRequest();
            }

            return Json(new{});
        }

        [HttpPost]
        [Route("/Follower/Unfollow/{id:int}")]
        [Route("/Follower/Nebesekti/{id:int}")]
        public async Task<IActionResult> Unfollow(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!await _followerService.Unfollow(user, id))
            {
                return BadRequest();
            }

            return Json(new {});
        }
    }
}