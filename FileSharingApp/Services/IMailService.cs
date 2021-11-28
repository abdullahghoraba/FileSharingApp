using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestViewModel mailRequest);
    }
}
