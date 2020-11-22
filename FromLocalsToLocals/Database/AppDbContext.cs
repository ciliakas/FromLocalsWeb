using FromLocalsToLocals.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;

namespace FromLocalsToLocals.Database
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){
            ChangeTracker.Tracked += OnEntityTracked;
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Review> Reviews { get; set; }


        private void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is Review review)
            {
                review.Date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            }
            
            else if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is Vendor vendor)
            {
                vendor.DateCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
