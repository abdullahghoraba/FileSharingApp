
using FileSharingApp.Areas.Admin.Models;
using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Services
{
    public interface IUserService
    {
        IQueryable<AdminUserViewModel> GetAll();
        IQueryable<AdminUserViewModel> GetBlockedUsers();
        IQueryable<AdminUserViewModel> Search(string term);
        Task<OperationResult> ToggleBlockUserAsync(string userId);
        Task<int> UserRegistrationCountAsync();
        Task<int> UserRegistrationCountAsync(int month);
        Task InitializeAsync();
    }
}
