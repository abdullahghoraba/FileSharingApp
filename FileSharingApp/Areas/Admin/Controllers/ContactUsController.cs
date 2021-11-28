using FileSharingApp.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Controllers
{
    public class ContactUsController : AdminBaseController
    {
        private readonly IContactUsService contactService;

        public ContactUsController(IContactUsService contactService)
        {
            this.contactService = contactService;
        }
        public async Task<IActionResult> Index()
        {
            var result =await contactService.GetAll().ToListAsync();
            return View(result);
        }
        //public async Task<IActionResult> Details()
        //{

        //}
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id,bool Status)
        {
            await contactService.ChangeStatusAsync(id, Status);
           return RedirectToAction("Index");
        }
    }
}
