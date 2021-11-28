using FileSharingApp.Data;
using FileSharingApp.Models;
using FileSharingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileSharingApp.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly IUploadService uploadService;
        private readonly IWebHostEnvironment environment;
        public string UserId
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }

        public UploadsController(IUploadService uploadService, IWebHostEnvironment environment)
        {
            this.uploadService = uploadService;
            this.environment = environment;
        }
        public IActionResult Index()
        {

            var result = uploadService.GetBy(UserId);
            return View(result);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int RequiredPage = 1)
        {
            var result = uploadService.GetAll();
            var model = await GetPagedData(result, RequiredPage);
            return View(model);
            
        }
        private async Task<List<UploadViewModel>> GetPagedData(IQueryable<UploadViewModel> result, int RequiredPage=1)
        {
            const int pageSize = 3;
            decimal rowsCount =await uploadService.GetUploadsCountAsync();
            var pagesCount = Math.Ceiling(rowsCount / pageSize); // to convert it to Decimal
            // because if he write in browser number of pages greater then the real page like 10
            if (RequiredPage > pagesCount)
            {
                RequiredPage = 1;
            }
            RequiredPage = RequiredPage <= 0 ? 1 : RequiredPage;
            int skipCount = (RequiredPage - 1) * pageSize;

            var pagedData =await result
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
            ViewBag.CurrentPage = RequiredPage;
            ViewBag.PagesCount = pagesCount;
            return pagedData;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(InputFile model)
        {
            if (ModelState.IsValid)
            {
                var newName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(model.File.FileName);
                var fileName = newName + extension;
                var root = environment.WebRootPath;
                var path = Path.Combine(root, "Uploads", fileName);

                using (var fileStream = System.IO.File.Create(path))
                {
                    await model.File.CopyToAsync(fileStream);
                }
                await uploadService.CreateAsync(new InputUpload()
                {
                    OriginalFileName = model.File.FileName,
                    FileName =fileName,
                    ContentType = model.File.ContentType,
                    Size = model.File.Length,
                    UserId = UserId

                });
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Results(string term, int RequiredPage=1)
        {
            var result = uploadService.Search(term);
            ViewBag.term = term;
            var model= await GetPagedData(result, RequiredPage);
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var selectedupload =await uploadService.FindAsync(id,UserId);
            if (selectedupload == null)
            {
                return NotFound();
            }

            return View(selectedupload);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var selectedupload = await uploadService.FindAsync(id,UserId);
            if (selectedupload == null)
            {
                return NotFound();
            }

            await uploadService.DeleteAsync(id, UserId);
            return RedirectToAction("Index");

        }
        [HttpGet]
        //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.None, NoStore = false)]
        public async Task<IActionResult> Download(string id)
        {
            var selectedFile = await uploadService.FindAsync(id);
            if (selectedFile == null)
            {
                return NotFound();
            }
            await uploadService.increamentDownloadCount(id);
  
            var path = "~/Uploads/" + selectedFile.FileName;

            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control", "no-cache,no-store");
            Response.Headers.Add("Clear-Site-Data", "cache");

            return File(path, selectedFile.ContentType, selectedFile.OriginalName);

        }
    }
}
