using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Utilities.Enums;
using System.Collections.Generic;

namespace FromLocalsToLocals.Web.ViewModels
{
    public class FeedVM
    {
        public AppUser ActiveUser { get; set; }
        public FeedTabs ActiveTab { get; set; }

        public IEnumerable<Post> Posts { get; set; } 

        public bool DisplayInDetails { get; set; }
        public Vendor Vendor { get; set; }
    }
}
