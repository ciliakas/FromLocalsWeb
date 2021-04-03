using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.Ado
{
    public interface IVendorServiceADO
    {
        Task UpdateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(Vendor vendor);
        Task InsertWorkHoursAsync(WorkHours workHours);
    }
}