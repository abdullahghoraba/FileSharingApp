using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingApp.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace FileSharingApp.Areas.Admin.Controllers
{
    public class ReportsController : AdminBaseController
    {
        private readonly IUserService userService;

        public ReportsController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Users()
        {
            var model = userService.GetAll().ToList();
            //  we need to show it as a pdf 
            return new ViewAsPdf(model);
        }
    }
}
