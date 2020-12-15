using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.Services
{
    public interface IVendorServiceADO
    {
        Task UpdateVendorAsync(Vendor vendor);
    }
}
