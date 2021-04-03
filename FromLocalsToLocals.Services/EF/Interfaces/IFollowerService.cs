using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.EF
{
    public interface IFollowerService
    {
        Task<bool> Follow(AppUser user, int? vendorId);
        Task<bool> Unfollow(AppUser user, int? vendorId);
    }
}