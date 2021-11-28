using AutoMapper;
using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp
{
    public class UploadProfile:Profile
    {
        public UploadProfile()
        {
            CreateMap<Models.InputUpload, Data.Uploads>()
                .ForMember(u=>u.Id,op=>op.Ignore())
                .ForMember(u => u.UploadDate, op => op.Ignore());
            CreateMap<Data.Uploads, Models.UploadViewModel>()
                .ForMember(u=>u.OriginalName,op=>op.MapFrom(c=>c.OriginalFileName));

         
        }
    }
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Data.ApplicationUser, Models.UserViewModel>()
                .ForMember(u=>u.HasPassword,op=>op.MapFrom(c=>c.PasswordHash!=null));
            CreateMap<Data.ApplicationUser, AdminUserViewModel>()
                .ForMember(x=>x.UserId,op=>op.MapFrom(y=>y.Id));
        }
    }

    public class ContactUsProfile : Profile
    {
        public ContactUsProfile()
        {

            CreateMap<Data.Contact, Models.ContactViewModel>();
        }
    }
}
