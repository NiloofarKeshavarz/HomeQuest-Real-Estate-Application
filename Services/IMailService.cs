using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeQuest.Models;

namespace HomeQuest.Services
{
    public interface IMailService
    {
            Task SendEmailAsync(MailRequest mailRequest);

    }
}