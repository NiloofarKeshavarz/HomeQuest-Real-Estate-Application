using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace HomeQuest.Models
{
    public class Favorite
    {
        public int Id { get; set; } //FIXME : remember to delete this when you set the DbContect for the relationship
        public int PropertyId{get; set; }
        public Property Property { get; set; }
        public int UserId{get; set; }
        public ApplicationUser User { get; set; }
    }
}