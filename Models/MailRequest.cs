using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQuest.Models
{
    public class MailRequest
    {
        //public string ToEmail { get; set; }
        public string UserEmail{get; set; }

        public string OfferAmount { get; set; }
        public string OfferMessage { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}