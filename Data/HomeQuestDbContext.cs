using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeQuest.Models;

namespace HomeQuest.Data
{
    public class HomeQuestDbContext :IdentityDbContext
    {
        public HomeQuestDbContext (DbContextOptions<HomeQuestDbContext> options) :base (options){ }
        public DbSet<Property> Properties { get; set; }  
        public DbSet<Image> Images { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{        base.OnModelCreating(modelBuilder);
         modelBuilder.Entity<Property>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Property)
            .HasForeignKey(i => i.PropertyId);
} 
        
    }
}