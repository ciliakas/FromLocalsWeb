using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.Services
{
    public interface IDataAdapterService
    {
        Task UpdateVendorAsync(Vendor vendor);
    }
}
