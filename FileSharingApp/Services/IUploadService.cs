using FileSharingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Services
{
    public interface IUploadService
    {
        IQueryable<UploadViewModel> GetAll();
        IQueryable<UploadViewModel> GetBy(string userId);
        IQueryable<UploadViewModel> Search(string term);
        Task CreateAsync(InputUpload model);
        Task<UploadViewModel> FindAsync(string id);

       Task<UploadViewModel> FindAsync(string id,string userId);
        Task DeleteAsync(string id, string userId);
        Task increamentDownloadCount(string id);
        Task<int> GetUploadsCountAsync();
    }
}
