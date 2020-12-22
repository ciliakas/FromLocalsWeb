using FromLocalsToLocals.Contracts.Entities;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public interface IFollowerService
    {
        Task<bool> Follow(AppUser user, int? vendorId);
        Task<bool> Unfollow(AppUser user, int? vendorId);

    }
}
