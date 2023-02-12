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
            
            // modelBuilder.Entity<Property>()
            //    .HasMany(p => p.Images)
            //    .WithOne(i => i.Property)
            //    .HasForeignKey(i => i.PropertyId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);


            // modelBuilder.Entity<Property>()
            //   .HasMany(p => p.Offers)
            //   .WithOne(i => i.Property)
            //   .HasForeignKey(i => i.PropertyId);


          modelBuilder.Entity<Property>()
               .HasMany(p => p.Images)
               .WithOne(i => i.Property)
               .HasForeignKey(i => i.PropertyId);

               // favorite
             modelBuilder.Entity<Favorite>()
            .HasOne<Property>(f => f.Property)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
            .HasOne<ApplicationUser>(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // property-agent
            modelBuilder.Entity<Property>()
            .HasOne<ApplicationUser>(p => p.Agent)
            .WithMany(a => a.Properties)
            .HasForeignKey(p => p.AgentId);

            // offer
            modelBuilder.Entity<Property>()
              .HasMany(p => p.Offers)
              .WithOne(o => o.Property)
              .HasForeignKey(i => i.PropertyId)
              .OnDelete(DeleteBehavior.Restrict);

             modelBuilder.Entity<Offer>()
                .HasOne(o => o.Agent)
                .WithMany(u => u.AgentOffers)
                .HasForeignKey(o => o.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

             modelBuilder.Entity<Offer>()
                .HasOne(o => o.User)
                .WithMany(u => u.UserOffers)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}