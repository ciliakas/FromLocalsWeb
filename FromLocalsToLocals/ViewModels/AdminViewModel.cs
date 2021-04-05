using System.Collections.Generic;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class AdminViewModel
    {
        public List<Report> Reports { get; set; }
        public int ReportId { get; set; }
        public int Category { get; set; }
    }
}
