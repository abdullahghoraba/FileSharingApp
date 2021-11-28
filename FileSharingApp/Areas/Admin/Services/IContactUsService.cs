using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Services
{
    public interface IContactUsService
    {
       IQueryable<ContactViewModel> GetAll();
        Task ChangeStatusAsync(int Id, bool Status);
    }
}
