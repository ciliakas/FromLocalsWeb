using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FromLocalsToLocals.Contracts.Entities
{
    public class Report
    {
        public Report()
        {
        }

        public Report(DateTime created, string userId, int category, string href)
        {
            CreatedDate = created;
            UserId = userId;
            Category = category;
            Href = href;
        }

        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public int Category { get; set; }
        public string Href { get; set; }

    }
}
