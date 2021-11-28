using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingApp.Data;
using FileSharingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly ApplicationContext _db;
        private readonly IMapper _mapper;

        public UploadService(ApplicationContext context,IMapper mapper)
        {
            this._db = context;
            this._mapper = mapper;
        }
        public async Task CreateAsync(InputUpload model)
        {
            var uploadsMapperObj = _mapper.Map<Uploads>(model);
            await _db.Uploads.AddAsync(uploadsMapperObj);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string userId)
        {
            var selectedUpload = await _db.Uploads.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            if (selectedUpload != null)
            {
                _db.Uploads.Remove(selectedUpload);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<UploadViewModel> FindAsync(string id, string userId)
        {
            var selectedUpload=await _db.Uploads.FirstOrDefaultAsync(u=> u.Id == id && u.UserId==userId );
            if (selectedUpload != null)
            {
                return _mapper.Map<UploadViewModel>(selectedUpload);
                //return new UploadViewModel
                //{
                //    Id = selectedUpload.Id,
                //    OriginalName = selectedUpload.OriginalFileName,
                //    FileName = selectedUpload.FileName,
                //    ContentType = selectedUpload.ContentType,
                //    Size = selectedUpload.Size,
                //    UploadDate = selectedUpload.UploadDate,

                //    DownloadCount = selectedUpload.DownloadCount
                //};
            }
            return null;
        }

        public async Task<UploadViewModel> FindAsync(string id)
        {
            var selectedUpload = await _db.Uploads.FirstOrDefaultAsync(u=>u.FileName==id);
            if (selectedUpload != null)
            {
                return _mapper.Map<UploadViewModel>(selectedUpload);
                //return new UploadViewModel
                //{
                //    Id = selectedUpload.Id,
                //    OriginalName = selectedUpload.OriginalFileName,
                //    FileName = selectedUpload.FileName,
                //    ContentType = selectedUpload.ContentType,
                //    Size = selectedUpload.Size,
                //    UploadDate = selectedUpload.UploadDate,

                //    DownloadCount = selectedUpload.DownloadCount
                //};
            }
            return null;
        }

        public IQueryable<UploadViewModel> GetAll()
        {
            var result = _db.Uploads
                .OrderByDescending(c => c.UploadDate)
                .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public IQueryable<UploadViewModel> GetBy(string userId)
        {
            var results = _db.Uploads.Where(x => x.UserId == userId).OrderByDescending(c => c.UploadDate)
                  .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);
            return results;
        }

        public async Task<int> GetUploadsCountAsync()
        {
            return await _db.Uploads.CountAsync();
        }

        public async Task increamentDownloadCount(string id)
        {
            var selectedFile =await _db.Uploads.FindAsync(id);
            if (selectedFile != null)
            {
                selectedFile.DownloadCount++;
                _db.Update(selectedFile);
                await _db.SaveChangesAsync();
            }

        }

        public IQueryable<UploadViewModel> Search(string term)
        {
            var result = _db.Uploads.OrderByDescending(c => c.UploadDate)
                .Where(x => x.OriginalFileName.Contains(term))
                   .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);
            return result;
        }
    }
}
