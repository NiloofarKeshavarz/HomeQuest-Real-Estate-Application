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
        //public virtual DbSet<IdentityUser> ApplicationUser {get; set;}


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

           // modelBuilder.Entity<Favorite>().HasKey(sc => new { sc.PropertyId, sc.UserId });

            // modelBuilder.Entity<Favorite>()
            //     .HasOne<Property>(sc => sc.Property)
            //     .WithMany(s => s.Favorite)
            //     .HasForeignKey(sc => sc.SId);


            // modelBuilder.Entity<StudentCourse>()
            //     .HasOne<Course>(sc => sc.Course)
            //     .WithMany(s => s.StudentCourses)
            //     .HasForeignKey(sc => sc.CId);
        }

    }
}