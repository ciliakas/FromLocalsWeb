using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using SuppLocals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Models;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VendorsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllVendors(string vendorType, string searchString)
        {
            IQueryable<string> typeQuery = from m in _context.Vendors
                                           orderby m.VendorTypeDb
                                           select m.VendorTypeDb;

            var vendors = from m in _context.Vendors
                        select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(vendorType))
            {
                vendors = vendors.Where(x => x.VendorTypeDb == vendorType);
            }
            var vendorTypeVM = new VendorTypeViewModel
            {
                Types = new SelectList(await typeQuery.Distinct().ToListAsync()),
                Vendors = await vendors.ToListAsync()
            };
            return View(vendorTypeVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MyVendors()
        {
            return View(await _context.Vendors.Where(x => x.UserID == _userManager.GetUserId(User)).ToListAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vendor model)
        {
            if (ModelState.GetFieldValidationState("Title") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Address") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                if (latLng != null)
                {

                    model.UserID = _userManager.GetUserId(User);
                    model.Latitude = latLng.Item1;
                    model.Longitude = latLng.Item2;
                    
                    _context.Vendors.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction("MyVendors");
                }
            }

            //Somehow we should inform the client-side that probably address is not recognizable
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vendor model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            var vendor = _context.Vendors.Single(x => x.ID == id);

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            if (ModelState.GetFieldValidationState("Title") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Address") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                try
                {
                    var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                    if (latLng == null)
                    {
                        //Somehow we should inform the client-side that probably address is not recognizable
                        return View(model);
                    }

                    vendor.Title = model.Title;
                    vendor.About = model.About;
                    vendor.Address = model.Address;
                    vendor.Latitude = latLng.Item1;
                    vendor.Longitude = latLng.Item2;
                    vendor.VendorType = model.VendorType;

                    _context.Update(vendor);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(model.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyVendors));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vendor == null)
            {
                return NotFound();
            }

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyVendors));
        }

        

        #region Helpers
        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.ID == id);
        }

        private bool ValidUser(string id)
        {
            return id == _userManager.GetUserId(User);
        }

        #endregion
    }
}
