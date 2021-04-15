using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class Report
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public int Category { get; set; }
        public string Href { get; set; }

    }
}
