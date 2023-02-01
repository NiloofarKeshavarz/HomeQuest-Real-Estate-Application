using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeQuest.Data
{
    public class HomeQuestDbContext :IdentityDbContext
    {
         public HomeQuestDbContext (DbContextOptions<HomeQuestDbContext> options) :base (options){ }
         //public DbSet<Article> Articles { get; set; }  
        
    }
}