using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromLocalsToLocals.Database;

namespace FromLocalsToLocals.Utilities
{
    public class SendAllSubscribers
    {
        private readonly AppDbContext _context;

        public SendAllSubscribers(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendingAll()
        {
            var users = _context.Users.ToList();
            var vendors = _context.Vendors;
            var newVendorsList = _context.Vendors.OrderByDescending(v => v.DateCreated).Take(3);

            var msg = new StringBuilder();

            if (vendors == null)
                msg.Append("At this time there is not new vendors");
            else
                foreach (var u in users)
                    if (u.Subscribe)
                    {
                        foreach (var v in newVendorsList)
                            msg.Append(v.Title + "    Vendor type: " + v.VendorType + " <br>");
                        await SendMail.NewsLetterSender(msg.ToString(), u.Email);
                    }
        }
    }
}