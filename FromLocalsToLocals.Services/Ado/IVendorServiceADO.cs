using FromLocalsToLocals.Contracts.Entities;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.Ado
{
    public interface IVendorServiceADO
    {
        Task UpdateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(Vendor vendor);
        Task InsertWorkHoursAsync(WorkHours workHours);
    }
}
