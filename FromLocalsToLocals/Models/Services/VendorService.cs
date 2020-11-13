using FromLocalsToLocals.Database;
using Geocoding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Models.Services
{
    public class VendorService : IVendorService
    {
        private readonly AppDbContext _context;

        public VendorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Vendor vendor)
        {
            try
            {
                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Unable to save service in database");
            }

        }

        public async Task DeleteAsync(Vendor vendor)
        {
            try
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Unable to delete service from database");
            }
        }
        public async Task<Vendor> GetVendorAsync(int id)
        {
            return await _context.Vendors.FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<List<Vendor>> GetVendorsAsync(string searchString="", string vendorType="")
        {
            var vendors = from v in _context.Vendors
                                          select v;
            return await FilterVendorsListAsync(vendors, searchString, vendorType);
        }

        public async Task<List<Vendor>> GetVendorsAsync(string userId, string searchString="",string vendorType="")
        {
            var vendors = from v in _context.Vendors
                          where v.UserID == userId
                                          select v;
            return await FilterVendorsListAsync(vendors,searchString,vendorType);
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            try
            {
                _context.Vendors.Update(vendor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Unable to update service in database");
            }

        }

        public bool Exists(int id)
        {
            return _context.Vendors.Any(e => e.ID == id);
        }

        private async Task<List<Vendor>> FilterVendorsListAsync(IQueryable<Vendor> vendors, string searchString = "", string vendorType = "")
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(vendorType))
            {
                vendors = vendors.Where(x => x.VendorTypeDb == vendorType);
            }
            return await vendors.ToListAsync();
        }
    
    }
}
