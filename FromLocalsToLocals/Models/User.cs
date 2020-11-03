using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FromLocalsToLocals.Models
{
    public class User : IEquatable<User>
    {
        public Location Location;


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required(ErrorMessage = "This field can not be empty")] 
        public string Username { get; set; }

        [Required(ErrorMessage = "This field can not be empty")] 
        public string HashedPsw { get; set; }

        [Required] public string Email { get; set; }

        public byte[] Image { get; set; }

        public int VendorsCount { get; set; }

        public bool Equals([AllowNull] User other)
        {
            if (other == null)
            {
                return false;
            }

            return (Username == other.Username);
        }

    }
}
