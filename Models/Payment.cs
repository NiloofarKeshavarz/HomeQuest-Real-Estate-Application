using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace HomeQuest.Models
{
    public class Payment
    {
        [Key]
        public int Id{get; set; }
        public int Amount{get; set; }

        public DateTime PaymentDate{get; set; }
        public ApplicationUser User{get; set; }


    }
}