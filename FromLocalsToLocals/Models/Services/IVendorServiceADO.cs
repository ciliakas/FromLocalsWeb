using FromLocalsToLocals.Contracts.Entities;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.Models.Services
{
    public interface IVendorServiceADO
    {
        Task UpdateVendorAsync(Vendor vendor);
    }
}
