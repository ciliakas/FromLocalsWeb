

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.Services
{
    public interface IVendorService
    {
        Task CreateAsync(Vendor vendor);
        Task<Vendor> GetVendorAsync(int id);
        Task<List<Vendor>> GetVendorsAsync(string searchString="", string vendorType = "");
        Task<List<Vendor>> GetVendorsAsync(string userId,string searchString="" , string vendorType="");
        Task UpdateAsync(Vendor vendor);
        Task DeleteAsync(Vendor vendor);
        bool Exists(int id);
        void Sort(List<Vendor> vendors, string order);
    }
}
