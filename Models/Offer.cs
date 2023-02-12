
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace HomeQuest.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int OfferAmount { get; set; }
        public DateTime OfferDate { get; set; }
        public string OfferMessage { get; set; }
        //public ApplicationUser User { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }

         public string UserId {get; set;}
        public virtual ApplicationUser User { get; set; }

         public string AgentId {get; set;}
        public virtual ApplicationUser Agent {get; set;}



    }
}