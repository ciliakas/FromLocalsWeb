using System;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.Ado;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Utilities;
using FromLocalsToLocals.Utilities.Enums;
using FromLocalsToLocals.Utilities.Helpers;
using FromLocalsToLocals.Web.Utilities;
using FromLocalsToLocals.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;

namespace FromLocalsToLocals.Web.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<VendorsController> _localizer;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<AppUser> _userManager;
        private readonly IVendorService _vendorService;
        private readonly IVendorServiceADO _vendorServiceADO;

        public VendorsController(AppDbContext context, UserManager<AppUser> userManager,
            IVendorService vendorService, IToastNotification toastNotification,
            IStringLocalizer<VendorsController> localizer, IVendorServiceADO dataAdapterService)
        {
            _context = context;
            _userManager = userManager;
            _vendorService = vendorService;
            _toastNotification = toastNotification;
            _localizer = localizer;
            _vendorServiceADO = dataAdapterService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllVendors([FromQuery(Name = "ordertype")] string? orderType,
            [FromQuery(Name = "vendortype")] string? vendorType,
            [FromQuery(Name = "searchString")] string? searchString, [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "itemCount")] int? itemCount)
        {
            var typesOfVendors = Enum.GetValues(typeof(VendorType)).Cast<VendorType>().ToList();
            var typesOfOrdering = Enum.GetValues(typeof(OrderType)).Cast<OrderType>().ToList();
            var newVendors = await _vendorService.GetNewVendorsAsync(4);

            var vendors = await _vendorService.GetVendorsAsync(searchString, vendorType);
            vendors.ForEach(x => x.CountAverage());
            _vendorService.Sort(vendors, orderType ?? "");


            var vendorTypeVM = new VendorTypeViewModel
            {
                Types = new SelectList(typesOfVendors),
                OrderTypes = new SelectList(typesOfOrdering),
                Vendors = PaginatedList<Vendor>.Create(vendors, page ?? 1, itemCount ?? 8),
                NewVendors = newVendors
            };

            return View(vendorTypeVM);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MyVendors()
        {
            return View(await _vendorService.GetVendorsAsync(userId: _userManager.GetUserId(User)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

            if (vendor == null) return NotFound();

            await _vendorService.UpdatePopularityAsync(vendor);

            try
            {
                vendor.FollowerCount = _context.Followers.Where(y => y.VendorID == vendor.ID).Count();
            }
            catch (Exception e)
            {
                await e.ExceptionSender();
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
                if (ModelState.GetFieldValidationState("Title") == ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("Address") == ModelValidationState.Valid)
                {
                    if (model.Image != null && !model.Image.ValidImage())
                    {
                        ModelState.AddModelError("", _localizer["Invalid image type"]);
                        _toastNotification.AddErrorToastMessage(_localizer["Invalid image type"]);
                        return View(model);
                    }

                    var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                    if (latLng == null)
                    {
                        ModelState.AddModelError("", _localizer["Sorry, we can't recognize this address"]);
                        _toastNotification.AddErrorToastMessage(_localizer["Sorry, we can't recognize this address"]);
                        return View(model);
                    }

                    var user = await _userManager.GetUserAsync(User);
                    user.VendorsCount++;
                    await _userManager.UpdateAsync(user);

                    var vendor = new Vendor();
                    vendor.UserID = user.Id;
                    vendor.Latitude = latLng.Item1;
                    vendor.Longitude = latLng.Item2;

                    model.SetValuesToVendor(vendor);
                    await _vendorService.CreateAsync(vendor);

                    var serviceOperatingHours = model.VendorHours;

                    foreach (var elem in serviceOperatingHours)
                        if (elem.IsWorking)
                        {
                            if (elem.CloseTime < elem.OpenTime)
                            {
                                ModelState.AddModelError("", "Invalid work hours");
                                _toastNotification.AddErrorToastMessage("Choose appropriate working hours");
                                await _vendorService.DeleteAsync(vendor);
                                return View(model);
                            }

                            var workHours = new WorkHours(vendor.ID, elem.IsWorking, elem.Day, elem.OpenTime,
                                elem.CloseTime);
                            await _vendorService.AddWorkHoursAsync(workHours);
                        }
                        else
                        {
                            var timeSpan = new TimeSpan(0);
                            var workHours = new WorkHours(vendor.ID, elem.IsWorking, elem.Day, timeSpan, timeSpan);
                            await _vendorServiceADO.InsertWorkHoursAsync(workHours);
                        }

                    var serviceListing = model.VendorListing;
                    foreach (var elem in serviceListing)
                    {
                        var vendorListing = new Listing(vendor.ID, elem.Name, elem.Price, elem.Image, elem.Description);
                        await _vendorService.AddListingAsync(vendorListing);
                    }


                    _toastNotification.AddSuccessToastMessage(_localizer["Service Created"]);
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
            if (id == null) return NotFound();

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

            if (vendor == null) return NotFound();

            if (!ValidUser(vendor.UserID)) return NotFound();

            var workHours = _context.VendorWorkHours.Where(x => x.VendorID == vendor.ID).OrderBy(y => y.Day).ToList();

            var vendorListing = _context.Listings.Where(x => x.VendorID == vendor.ID).OrderBy(y => y.ListingID).ToList();

            return View(new CreateEditVendorVM(vendor, workHours, vendorListing));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEditVendorVM model)
        {
            if (id != model.ID) return NotFound();

            var vendor = await _vendorService.GetVendorAsync(id);

            if (!ValidUser(vendor.UserID)) return NotFound();

            if (ModelState.GetFieldValidationState("Title") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Address") == ModelValidationState.Valid)
            {
                if (model.Image != null && !model.Image.ValidImage())
                {
                    ModelState.AddModelError("", _localizer["Invalid image type"]);
                    _toastNotification.AddErrorToastMessage(_localizer["Invalid image type"]);
                    return View(model);
                }

                var latLng = await MapMethods.ConvertAddressToLocationAsync(model.Address);

                if (latLng == null)
                {
                    _toastNotification.AddErrorToastMessage(_localizer["Sorry, we can't recognize this address"]);
                    return View(model);
                }

                try
                {
                    vendor.Latitude = latLng.Item1;
                    vendor.Longitude = latLng.Item2;

                    model.SetValuesToVendor(vendor);

                    var serviceOperatingHours = model.VendorHours;
                    foreach (var elem in serviceOperatingHours)
                        if (elem.IsWorking)
                        {
                            if (elem.CloseTime < elem.OpenTime)
                            {
                                ModelState.AddModelError("", "Invalid work hours");
                                _toastNotification.AddErrorToastMessage("Choose appropriate working hours");
                                return View(model);
                            }

                            var workHours = new WorkHours(vendor.ID, elem.IsWorking, elem.Day, elem.OpenTime,
                                elem.CloseTime);
                            await _vendorService.ChangeWorkHoursAsync(workHours);
                        }
                        else
                        {
                            var timeSpan = new TimeSpan(0);
                            var workHours = new WorkHours(vendor.ID, elem.IsWorking, elem.Day, timeSpan, timeSpan);
                            await _vendorService.ChangeWorkHoursAsync(workHours);
                        }

                    await _vendorServiceADO.UpdateVendorAsync(vendor);
                    _toastNotification.AddSuccessToastMessage(_localizer["Service Updated"]);

                    return RedirectToAction(nameof(MyVendors));
                }
                catch (Exception ex)
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
            if (id == null) return NotFound();

            var vendor = await _vendorService.GetVendorAsync(id ?? default);

            if (vendor == null) return NotFound();

            if (!ValidUser(vendor.UserID)) return NotFound();

            try
            {
                try
                {
                    _context.Notifications.RemoveRange(_context.Notifications.Where(x => x.VendorId == vendor.ID));
                    await _vendorServiceADO.DeleteVendorAsync(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    await e.ExceptionSender();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _toastNotification.AddErrorToastMessage(ex.Message);
            }

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