using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace HomeQuest.Models
{
    public class MailData
    {
        public string To { get; }
        public string Subject { get; }
        public string? Body { get; }
        public IFormFileCollection? Attachments { get; set; }

        public MailData(string to, string subject, string? body = null, IFormFileCollection? attachments = null)
        {
            To = to;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }
    }
}