using FromLocalsToLocals.Database;
using FromLocalsToLocals.Utilities;
using Geocoding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FromLocalsToLocals.Models
{
    public class Vendor : IEquatable<Vendor>, IComparable<Vendor>
    {
        public List<Review> Reviews = new List<Review>();

        [NotMapped] public Location Location { get; set; }

        public int[] ReviewsCount = {0, 0, 0, 0, 0};

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required] public string UserID { get; set; }
          
        [Required] 
        public string Title { get; set; }

        public string About { get; set; }

        [Required] 
        public string Address { get; set; }

        [Required] public double Latitude { get; set; }

        [Required] public double Longitude { get; set; }


        [Column("VendorType")]
        [DisplayName("Vendor Type")]
        public string VendorTypeDb
        {
            get => VendorType.ToString();
            private set { VendorType = value.ParseEnum<VendorType>(); }
        }

        [NotMapped]
        public VendorType VendorType { get; set; }
        public byte[] Image { get; set; }

        #region IComparable


        public int CompareTo(Vendor other)
        {
            if (other == null) return 1;

            return ReviewsCount.Average().CompareTo(other.ReviewsCount.Average());
        }

        public static bool operator >(Vendor operand1, Vendor operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        public static bool operator <(Vendor operand1, Vendor operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        public static bool operator >=(Vendor operand1, Vendor operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        public static bool operator <=(Vendor operand1, Vendor operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        #endregion

        #region IEquatable

        public bool Equals([AllowNull] Vendor other)
        {
            if (other == null)
            {
                return false;
            }

            return (Title == other.Title);
        }

        #endregion

        public void UpdateReviewsCount(AppDbContext context)
        {
            var reviews = context.Reviews.Where(x => x.VendorID == ID).ToList();

            for (var i = 0; i < 5; i++)
            {
                ReviewsCount[i] = 0;
            }

            reviews.ForEach(x => ReviewsCount[x.Stars - 1]++);
        }

        public double Average()
        {
            var sum = 0;
            for (var i = 0; i < 5; i++)
            {
                sum += ReviewsCount[i] * (i+1);
            }

            if (ReviewsCount.Sum() == 0)
            {
                return 0;
            }

            return sum / (double)ReviewsCount.Sum();
        }
    }
}
