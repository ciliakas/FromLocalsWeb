using System.Collections.Generic;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.EF
{
    public interface IVendorService
    {
        Task AddPostAsync(Vendor vendor, Post post);
        Task CreateAsync(Vendor vendor);

        Task<Vendor> GetVendorAsync(int id);
        Task<Vendor> GetVendorAsync(string userId, string title);
        Task<List<Vendor>> GetVendorsAsync(string searchString = "", string vendorType = "");
        Task<List<Vendor>> GetVendorsAsync(string userId, string searchString = "", string vendorType = "");
        Task<List<Vendor>> GetNewVendorsAsync(int count);
        Task<List<Vendor>> GetPopularVendorsAsync(int count);
        Task UpdatePopularityAsync(Vendor vendor);

        Task AddWorkHoursAsync(WorkHours workHours);
        Task ChangeWorkHoursAsync(WorkHours workHours);


        Task UpdateAsync(Vendor vendor);
        Task DeleteAsync(Vendor vendor);
        bool Exists(int id);
        void Sort(List<Vendor> vendors, string order);
    }
}