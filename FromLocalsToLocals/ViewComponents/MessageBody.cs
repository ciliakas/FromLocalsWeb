using FromLocalsToLocals.Database;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FromLocalsToLocals.ViewComponents
{
    public class MessageBody : ViewComponent
    {
        private readonly AppDbContext _context;

        public MessageBody(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> InvokeAsync()
        {
            return null;
        }
    }
}
