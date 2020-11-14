using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using SuppLocals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using FromLocalsToLocals.Utilities;
using FromLocalsToLocals.Models.Services;
using NToastNotify;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IVendorService _vendorService;
        private readonly IToastNotification _toastNotification;

        public VendorsController(UserManager<AppUser> userManager,IVendorService vendorService,IToastNotification toastNotification)
        {
            _userManager = userManager;
            _vendorService = vendorService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllVendors(string vendorType, string searchString)
        {
            List<VendorType> typesOfVendors = Enum.GetValues(typeof(VendorType)).Cast<VendorType>().ToList();

            var vendorTypeVM = new VendorTypeViewModel
            {
                Types = new SelectList(typesOfVendors),
                Vendors = await _vendorService.GetVendorsAsync(searchString, vendorType),
            };

            return View(vendorTypeVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MyVendors()
        {
            return View(await _vendorService.GetVendorsAsync( userId : _userManager.GetUserId(User)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

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

                    await _vendorService.CreateAsync(model);

                    _toastNotification.AddSuccessToastMessage("Service Created");

                    return RedirectToAction("MyVendors");
                }
            }

            ModelState.AddModelError("", "Sorry, we can't recognize this address");
            _toastNotification.AddErrorToastMessage("Sorry, we can't recognize this address");

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

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

            var vendor = await _vendorService.GetVendorAsync(id);

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            if (ModelState.GetFieldValidationState("Title") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Address") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {   

                var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                if (latLng == null)
                {
                    _toastNotification.AddErrorToastMessage("Sorry, we can't recognize this address");
                    return View(model);
                }

                vendor.Title = model.Title;
                vendor.About = model.About;
                vendor.Address = model.Address;
                vendor.Latitude = latLng.Item1;
                vendor.Longitude = latLng.Item2;
                vendor.VendorType = model.VendorType;


                await _vendorService.UpdateAsync(vendor);
                _toastNotification.AddSuccessToastMessage("Service Updated");

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

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

            if (vendor == null)
            {
                return NotFound();
            }

            if (!ValidUser(vendor.UserID))
            {
                return NotFound();
            }

            await _vendorService.DeleteAsync(vendor);

            return RedirectToAction(nameof(MyVendors));
        }
        
        #region Helpers

        private bool ValidUser(string id)
        {
            return id == _userManager.GetUserId(User);
        }

        #endregion
    }
}
