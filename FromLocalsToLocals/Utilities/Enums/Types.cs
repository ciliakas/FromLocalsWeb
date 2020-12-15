using FromLocalsToLocals;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FromLocalsToLocals.Utilities
{

    [TypeConverter(typeof(Resources.VendorTypeEnum))]
    public enum VendorType
    {
        [Display(Name = "Food", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Food,
        [Display(Name = "CarRepair",Description = "Car Repair",ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        CarRepair,
        [Display(Name = "Beverages", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Beverages,
        [Display(Name = "Clothing", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Clothing,
        [Display(Name = "Electronics", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Electronics,
        [Display(Name = "RuralTourism", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        RuralTourism,
        [Display(Name = "Footwear", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Footwear,
        [Display(Name = "Stationery", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Stationery,
        [Display(Name = "Toys", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Toys,
        [Display(Name = "Music", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Music,
        [Display(Name = "Flowers", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Flowers,
        [Display(Name = "Other", ResourceType = typeof(FromLocalsToLocals.Resources.VendorTypeEnum))]
        Other
    }
    public enum OrderType
    {
        MostLiked,
        LeastLiked
    }


}
