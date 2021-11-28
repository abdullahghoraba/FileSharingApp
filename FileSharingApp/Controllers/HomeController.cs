using FileSharingApp.Data;
using FileSharingApp.Helper.Email;
using FileSharingApp.Hubs;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;
        private readonly IMailHelper _mailHelper;
        private readonly IHubContext<NotificationHub> notifications;
        private readonly UserManager<ApplicationUser> userManager;

        public string UserId { get {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            } }

        public HomeController(ILogger<HomeController> logger,
            ApplicationContext context,IMailHelper mailHelper,
            IHubContext<NotificationHub> notifications,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this._db = context;
            this._mailHelper = mailHelper;
            this.notifications = notifications;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var highstDownloads = _db.Uploads
                .OrderByDescending(x => x.DownloadCount)
                .ThenByDescending(c=>c.UploadDate).Take(3).Select(u => new UploadViewModel()
            {
                Id = u.Id,
                OriginalName = u.OriginalFileName,
                FileName = u.FileName,
                ContentType = u.ContentType,
                Size = u.Size,
                UploadDate = u.UploadDate,

                DownloadCount = u.DownloadCount

            });
            ViewBag.Popular = highstDownloads;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(new Contact()
                {
                    Email = model.Email,
                    Message = model.Message,
                    Name = model.Name,
                    Subject = model.Subject,
                    UserId = UserId
                });
                await _db.SaveChangesAsync();
                TempData["Message"] = "the data has been Sent successfully ";
                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine();
                bodyBuilder.AppendFormat("Name {0} :", model.Name);
               
                // Send Mail
                _mailHelper.SendEmail(new InputEmailMessage()
                {
                    Body = "Test",
                    Email = model.Email,
                    Subject = "you have unread message"
                });
                var adminUsers = await userManager.GetUsersInRoleAsync(UserRoles.Admin);
                var adminIdes = adminUsers.Select(x => x.Id);
                if (adminIdes.Any())
                {
                    await notifications.Clients.Users(adminIdes)
                        .SendAsync("ReceivedNotificication", "you have unread message from Client :" + model.Email);

                }
                return RedirectToAction("Contact");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SetCulture(string lang,string returnUrl=null)
        {
            if(!string.IsNullOrEmpty(lang))
            {
                // we can set lang in queryString or cookie or header
                // the better choice cookie because you don't need to send it every 
                // time you want to work 
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
               
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
    }
}
