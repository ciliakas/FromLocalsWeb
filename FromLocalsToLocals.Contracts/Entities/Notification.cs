using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotiId { get; set; }
        public string OwnerId { get; set; }
        public int VendorId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NotiBody { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Review Review { get; set; }

        [NotMapped]
        public bool IsRead { get; set; } = false;

    }
}
