using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FromLocalsToLocals.Utilities.Resources;

namespace FromLocalsToLocals.Utilities.Enums
{
    [TypeConverter(typeof(VendorTypeEnum))]
    public enum VendorType
    {
        [Display(Name = "Food", ResourceType = typeof(VendorTypeEnum))]
        Food,

        [Display(Name = "CarRepair", Description = "Car Repair", ResourceType = typeof(VendorTypeEnum))]
        CarRepair,

        [Display(Name = "Beverages", ResourceType = typeof(VendorTypeEnum))]
        Beverages,

        [Display(Name = "Clothing", ResourceType = typeof(VendorTypeEnum))]
        Clothing,

        [Display(Name = "Electronics", ResourceType = typeof(VendorTypeEnum))]
        Electronics,

        [Display(Name = "RuralTourism", ResourceType = typeof(VendorTypeEnum))]
        RuralTourism,

        [Display(Name = "Footwear", ResourceType = typeof(VendorTypeEnum))]
        Footwear,

        [Display(Name = "Stationery", ResourceType = typeof(VendorTypeEnum))]
        Stationery,

        [Display(Name = "Toys", ResourceType = typeof(VendorTypeEnum))]
        Toys,

        [Display(Name = "Music", ResourceType = typeof(VendorTypeEnum))]
        Music,

        [Display(Name = "Flowers", ResourceType = typeof(VendorTypeEnum))]
        Flowers,

        [Display(Name = "Other", ResourceType = typeof(VendorTypeEnum))]
        Other
    }

    public enum OrderType
    {
        MostLiked,
        LeastLiked
    }
}