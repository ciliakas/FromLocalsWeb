using FromLocalsToLocals.Database;
using FromLocalsToLocals.Utilities;
using Geocoding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FromLocalsToLocals.Models
{
    public class Vendor : IEquatable<Vendor>, IComparable<Vendor>
    {
        [NotMapped] public Location Location { get; set; }

        public int[] ReviewsCount = new int[5];

        [NotMapped]
        public double ReviewsAverage 
        {
            get => CountAverage();
            set => ReviewsAverage = value;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required] public string UserID { get; set; }
          
        [Required]
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public string DateCreated { get; set; }

        [Required] public double Latitude { get; set; }

        [Required] public double Longitude { get; set; }


        [Column("VendorType")]
        [DisplayName("Vendor Type")]
        public string VendorTypeDb
        {
            get => VendorType.ToString();
            private set { VendorType = value.ParseEnum<VendorType>(); }
        }

        public int Popularity { get; set; }
        public DateTime LastClickDate { get; set; }

        [NotMapped]
        public VendorType VendorType { get; set; }

        public byte[] Image { get; set; }

        [JsonIgnore] 
        [IgnoreDataMember]
        [ForeignKey("UserID")]
        public virtual AppUser User { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Review> Reviews { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Post> Posts { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Follower> Followers { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<WorkHours> VendorHours { get; set; }

        #region IComparable


        public int CompareTo(Vendor other)
        {
            if (other == null) return 1;

            return ReviewsAverage.CompareTo(other.ReviewsAverage);
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

        public double CountAverage()
        {
            if (Reviews.Count == 0)
            {
                return 0;
            }
                
            var groupedRatings = Reviews.GroupBy(
                             rev => rev.Stars,
                             rev => rev.Stars,
                             (star, arr) => new
                             {
                                 Star = star,
                                 Count = arr.Count()
                             });

            foreach(var rating in groupedRatings)
            {
                ReviewsCount[rating.Star-1] = rating.Count; 
            }
            
            return  groupedRatings.Aggregate(0,(result,element) => result + element.Star * element.Count)/(double)Reviews.Count;
        }
    }
}
