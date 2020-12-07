using FromLocalsToLocals.Database;
using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class ContactBody : ViewComponent
    {
        private readonly AppDbContext _context;

        public ContactBody(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> InvokeAsync(AppUser user)
        {
            try
            {
            }
            catch (Exception)
            {

            }
            return null;

        }

    }
}
