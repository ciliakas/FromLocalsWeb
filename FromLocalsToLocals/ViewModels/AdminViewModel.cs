using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class AdminViewModel
    {
        public List<Report> Reports { get; set; }

        public int Category { get; set; }
    }
}
