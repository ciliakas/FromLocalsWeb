using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FromLocalsToLocals.Services.EF
{
    public class FollowerService : IFollowerService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FollowerService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Follow(AppUser user, int? vendorId)
        {
            if (vendorId == null || user == null
                                 || user.Vendors.Any(x => x.ID == vendorId)
                                 || user.Following.Any(x => x.VendorID == vendorId))
                return false;

            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.ID == vendorId);
            var follower = new Follower(user, vendor);
            try
            {
                user.Following.Add(follower);
                await _userManager.UpdateAsync(user);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Unfollow(AppUser user, int? vendorId)
        {
            if (vendorId == null || user == null
                                 || !user.Following.Any(x => x.VendorID == vendorId))
                return false;

            try
            {
                var followingVendor = user.Following.FirstOrDefault(x => x.VendorID == vendorId);
                user.Following.Remove(followingVendor);
                await _userManager.UpdateAsync(user);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}