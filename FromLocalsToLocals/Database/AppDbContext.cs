using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FromLocalsToLocals.Database
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}

        //public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Review> Reviews { get; set; }


    }
}
