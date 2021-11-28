using ClosedXML.Excel;
using FileSharingApp.Areas.Admin.Models;
using FileSharingApp.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IUserService userService;
        private readonly IXLWorkbook workbook;

        public UserController(IUserService userService,
            IXLWorkbook workbook)
        {
            this.userService = userService;
            this.workbook = workbook;
        }
        public async Task<IActionResult> Index()
        {
            var result = await userService.GetAll()
                .ToListAsync();
            return View(result);
        }
        public async Task<IActionResult> Blocked()
        {
            var result = await userService.GetBlockedUsers()
                  .ToListAsync();
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Search(InputSearch model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.Search(model.Term)
                      .ToListAsync();
                return View("Index", result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Block(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var result = await userService.ToggleBlockUserAsync(userId);
                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
                return RedirectToAction("Index");

            }
            TempData["Error"] = "User Id is Not Found";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UsersCount()
        {

            var totalUserCounts = await userService.UserRegistrationCountAsync();
            var month = DateTime.Today.Month;
            var totalUserCountsByMonth = await userService.UserRegistrationCountAsync(month);
            return Json(new { total = totalUserCounts, month = totalUserCountsByMonth });
        }
        public async Task<IActionResult> ExportToExcel()
        {
            var contentTypeForExcelFile = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Users.xlsx";
            var result = await userService.GetAll().ToListAsync();
            var workSheet = workbook.Worksheets.Add("All Users");
            workSheet.Cell(1, 1).Value = "First Name";
            workSheet.Cell(1, 2).Value = "Last Name";
            workSheet.Cell(1, 3).Value = "Email";
            workSheet.Cell(1, 4).Value = "Created Date";
            for (int i = 1; i < result.Count; i++)
            {
                var row = i + 1;
                workSheet.Cell(row, 1).Value = result[i - 1].FirstName;
                workSheet.Cell(row, 2).Value = result[i - 1].LastName;
                workSheet.Cell(row, 3).Value = result[i - 1].Email;
                workSheet.Cell(row, 4).Value = result[i - 1].CreatedDate;
            }
            using(var ms=new MemoryStream())
            {
                workbook.SaveAs(ms);
                return File(ms.ToArray(), contentTypeForExcelFile, fileName);
            }

        }
    }
}
