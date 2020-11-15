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
using FromLocalsToLocals.Models.ViewModels;
using System.IO;

namespace FromLocalsToLocals.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private readonly Lazy<UserManager<AppUser>> _userManager;
        private readonly IVendorService _vendorService;
        private readonly IToastNotification _toastNotification;
        private readonly AppDbContext _context;
 
        public VendorsController(AppDbContext context,UserManager<AppUser> userManager,IVendorService vendorService,IToastNotification toastNotification)
        {
            _userManager = new Lazy<UserManager<AppUser>>(() => userManager);
            _vendorService = vendorService;
            _toastNotification = toastNotification;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllVendors([FromQuery(Name = "ordertype")] string? orderType,[FromQuery(Name = "vendortype")]string? vendorType, [FromQuery(Name = "searchString")] string? searchString, [FromQuery(Name = "page")] int? page, [FromQuery(Name = "itemCount")] int? itemCount)
        {
            List<VendorType> typesOfVendors = Enum.GetValues(typeof(VendorType)).Cast<VendorType>().ToList();
            List<OrderType> typesOfOrdering = Enum.GetValues(typeof(OrderType)).Cast<OrderType>().ToList();

            var vendors = await _vendorService.GetVendorsAsync(searchString,vendorType);
            vendors.ForEach(a => a.UpdateReviewsCount(_context));
            _vendorService.Sort(vendors, orderType ?? "");


            var vendorTypeVM = new VendorTypeViewModel
            {
                Types = new SelectList(typesOfVendors),
                OrderTypes = new SelectList(typesOfOrdering),
                Vendors = PaginatedList<Vendor>.Create(vendors, page ?? 1, itemCount ?? 20)
            };

            return View(vendorTypeVM);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MyVendors()
        {
            return View(await _vendorService.GetVendorsAsync( userId : _userManager.Value.GetUserId(User)));
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
        public async Task<IActionResult> Create(CreateEditVendorVM model)
        {
            try
            {
                if (ModelState.GetFieldValidationState("Title") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("Address") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    if (model.Image != null & !model.Image.ValidImage())
                    {
                        ModelState.AddModelError("", "Invalid image type");
                        _toastNotification.AddErrorToastMessage("Invalid image type");
                        return View(model);
                    }

                    var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                    if (latLng == null)
                    {
                        ModelState.AddModelError("", "Sorry, we can't recognize this address");
                        _toastNotification.AddErrorToastMessage("Sorry, we can't recognize this address");
                        return View(model);
                    }

                    var vendor = new Vendor();
                    vendor.UserID = _userManager.Value.GetUserId(User);
                    vendor.Latitude = latLng.Item1;
                    vendor.Longitude = latLng.Item2;

                    model.SetValuesToVendor(vendor);

                    await _vendorService.CreateAsync(vendor);

                    _toastNotification.AddSuccessToastMessage("Service Created");
                    return RedirectToAction("MyVendors");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _toastNotification.AddErrorToastMessage(ex.Message);
            }
            
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

            return View(new CreateEditVendorVM(vendor));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEditVendorVM model)
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
                if (model.Image != null & !model.Image.ValidImage())
                {
                    ModelState.AddModelError("", "Invalid image type");
                    _toastNotification.AddErrorToastMessage("Invalid image type");
                    return View(model);
                }

                var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                if (latLng == null)
                {
                    _toastNotification.AddErrorToastMessage("Sorry, we can't recognize this address");
                    return View(model);
                }

                try
                {
                    vendor.Latitude = latLng.Item1;
                    vendor.Longitude = latLng.Item2;

                    model.SetValuesToVendor(vendor);

                    await _vendorService.UpdateAsync(vendor);
                    _toastNotification.AddSuccessToastMessage("Service Updated");

                    return RedirectToAction(nameof(MyVendors));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _toastNotification.AddErrorToastMessage(ex.Message);
                }

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

            try
            {
                await _vendorService.DeleteAsync(vendor);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _toastNotification.AddErrorToastMessage(ex.Message);
            }
            return RedirectToAction(nameof(MyVendors));
        }
        
        #region Helpers

        private bool ValidUser(string id)
        {
            return id == _userManager.Value.GetUserId(User);
        }

        #endregion
    }
}
