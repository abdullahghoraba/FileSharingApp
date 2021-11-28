using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingApp.Areas.Admin.Models;
using FileSharingApp.Data;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(ApplicationContext context, IMapper mapper,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this._mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<OperationResult> ToggleBlockUserAsync(string userId)
        {
            var existedUser = await context.Users.FindAsync(userId);
            if (existedUser == null)
            {
                return OperationResult.NotFound();
            }
            existedUser.IsBlocked = !existedUser.IsBlocked;
            context.Update(existedUser);
            await context.SaveChangesAsync();
            return OperationResult.Sucesseded();
        }

        public IQueryable<AdminUserViewModel> GetAll()
        {
            var result = context.Users
                .OrderByDescending(x => x.CreatedDate)
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public IQueryable<AdminUserViewModel> GetBlockedUsers()
        {
            var result = context.Users.Where(x => x.IsBlocked)
                .OrderByDescending(x => x.CreatedDate)
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public IQueryable<AdminUserViewModel> Search(string term)
        {
            var result = context.Users.Where(x => (x.Email == term ||
            x.FirstName.Contains(term) ||
            x.LastName.Contains(term) ||
            (x.FirstName + " " + x.LastName).Contains(term)
            )).OrderByDescending(x => x.CreatedDate)
              .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public async Task<int> UserRegistrationCountAsync()
        {
            return await context.Users.CountAsync();
        }

        public async Task<int> UserRegistrationCountAsync(int month)
        {
            var NowYear = DateTime.Today.Year;
            var count = await context.Users.CountAsync(x =>
             x.CreatedDate.Month == month
             && x.CreatedDate.Year == NowYear);
            return count;
        }

        public async Task InitializeAsync()
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            var adminEmail = "admin@a.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {

                var adminAccount = new ApplicationUser()
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    PhoneNumber="01020390074",
                    EmailConfirmed=true,
                    PhoneNumberConfirmed=true
                    
                };
                await userManager.CreateAsync(adminAccount, "Pa$$w0rd");
                await userManager.AddToRoleAsync(adminAccount, UserRoles.Admin);
            }
        }
    }
}
