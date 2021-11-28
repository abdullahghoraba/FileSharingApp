using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingApp.Data;
using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;

        public ContactUsService(ApplicationContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task ChangeStatusAsync(int Id, bool Status)
        {
            var selectedItem =await context.Contacts.FindAsync(Id);
            if (selectedItem != null)
            {
                selectedItem.IsClosed = Status;
                context.Update(selectedItem);
                await context.SaveChangesAsync();
            }
        }

        public IQueryable<ContactViewModel> GetAll()
        {
           var result= context.Contacts.ProjectTo<ContactViewModel>(mapper.ConfigurationProvider);
            return result;
        }
    }
}
