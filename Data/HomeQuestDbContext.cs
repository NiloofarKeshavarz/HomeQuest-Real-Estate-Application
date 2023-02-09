using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeQuest.Models;

namespace HomeQuest.Data
{
    public class HomeQuestDbContext : IdentityDbContext<ApplicationUser>
    {
        public HomeQuestDbContext(DbContextOptions<HomeQuestDbContext> options) : base(options) { }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Property>()
               .HasMany(p => p.Images)
               .WithOne(i => i.Property)
               .HasForeignKey(i => i.PropertyId);


            // modelBuilder.Entity<Property>()
            //   .HasMany(p => p.Offers)
            //   .WithOne(i => i.Property)
            //   .HasForeignKey(i => i.PropertyId);

           modelBuilder.Entity<Favorite>().HasKey(f => new { f.PropertyId, f.UserId });

            modelBuilder.Entity<Favorite>()
                .HasOne<Property>(f => f.Property)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.PropertyId);


            modelBuilder.Entity<Favorite>()
                .HasOne<ApplicationUser>(f => f.User)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.UserId);
        }

    }
}