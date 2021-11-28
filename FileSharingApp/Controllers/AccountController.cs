using AutoMapper;
using FileSharingApp.Data;
using FileSharingApp.Helper.Email;
using FileSharingApp.Models;
using FileSharingApp.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileSharingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper _mapper;
        private readonly IMailHelper _mailHelper;
        private readonly IMailService mailService;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IMapper mapper, 
            IMailHelper mailHelper,IMailService mailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this._mapper = mapper;
            this._mailHelper = mailHelper;
            this.mailService = mailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (existingUser == null)
                {
                    TempData["Error"] = "Invalid Email Or Password";
                    return View(loginViewModel);
                }
                if (!existingUser.IsBlocked)
                {
                    var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                           //Remember Me
                           true,
                           // if he try to login fivetimes then fail close the Account or not
                           true);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }
                        return RedirectToAction("Create", "Uploads");
                    }
                    else if (result.IsNotAllowed)
                    {
                        TempData["Error"] = Resources.Shared.Error;
                    }
                }
                else
                {
                    TempData["Error"] = "This account has been blocked";
                }
            }
            return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName

                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    //create link for confirmation password
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", new { token = token, user = user.Id },
                        // this option to add domain to url with confirmation token like ourdomai.com/Account/ConfirmEmail
                        // 
                        Request.Scheme);
                    // send Email
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("File Sharing Application : Email Confirmation ");
                    body.AppendFormat("to confirm your Email ,you should <a href='{0}>Click here</a>' ", url);
                    //_mailHelper.SendEmail(new InputEmailMessage()
                    //{
                    //    Body = body.ToString(),
                    //    Email = registerViewModel.Email,
                    //    Subject = "Email Confirmation Message"
                    //});
                    await mailService.SendEmailAsync(new MailRequestViewModel()
                    {
                        Attachments = null,
                        Body=body.ToString(),
                        Subject= "Email Confirmation Message",
                        ToEmail=registerViewModel.Email

                    });
                    // we must add page here to confirm email
                    return RedirectToAction("RequiredEmailConfirm");

                    //await signInManager.SignInAsync(user, true);

                    //return RedirectToAction("Create", "Uploads");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult RequiredEmailConfirm()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ExternalLogin(string provider)  // provider looklike : "Facebook" "Google"
                                                             // //FacebookDefaults.AuthenticationScheme if you forgot facebook name 
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, "/Account/ExternalResponse");
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalResponse()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Message"] = "Login Failed";
                return RedirectToAction("Login");
            }

            var loginResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (!loginResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                //Create local Account
                var userToCreate = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true

                };
                var createResult = await userManager.CreateAsync(userToCreate);
                if (createResult.Succeeded)
                {
                    var externalLoginResult = await userManager.AddLoginAsync(userToCreate, info); //AspNetUserLogin Table
                    if (externalLoginResult.Succeeded)
                    {
                        await signInManager.SignInAsync(userToCreate, false, info.LoginProvider);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await userManager.DeleteAsync(userToCreate);
                    }
                }
                RedirectToAction("Login");
            }

            if (info.Principal.HasClaim(c=>c.Type==ClaimTypes.Email))
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    TempData["Error"] = "Invalid Email Or Password";
                    return RedirectToAction("Login");
                }
                if (existingUser.IsBlocked)
                {
                    await signInManager.SignOutAsync();
                    TempData["Error"] = "This account has been blocked";
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var usermodel = _mapper.Map<UserViewModel>(currentUser);
                return View(usermodel);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Info(UserViewModel model)
        {
            if (ModelState.IsValid)
            {


                var currentUser = await userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    currentUser.FirstName = model.FirstName;
                    currentUser.LastName = model.LastName;
                    var result = await userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = Resources.Shared.SuccessMessage;
                        return RedirectToAction("Info");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }


                }
                else
                {
                    return NotFound();
                }

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var result = await userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = Resources.Shared.ChangePasswordMessage;
                        await signInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return View("Info", _mapper.Map<UserViewModel>(currentUser));
        }
        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var result = await userManager.AddPasswordAsync(currentUser, model.Password);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = Resources.Shared.AddPasswordMessage;
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return View("Info", _mapper.Map<UserViewModel>(currentUser));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        var result = await userManager.ConfirmEmailAsync(user, model.Token);
                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Your Email is confirmed successfully! ";
                            return RedirectToAction("Login");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Your Email is already Confirmed ";
                    }
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var url = Url.Action("ResetPassword", "Account", new { token, model.Email }, Request.Scheme);
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("File Sharing : Reset Password ");
                    body.AppendLine("we are sending this email ,because we have recived a reset password request ");
                    body.AppendFormat("to reset new password <a href='{0}'>Click this link </a>", url);
                    //_mailHelper.SendEmail(new InputEmailMessage()
                    //{
                    //    Email = model.Email,
                    //    Body = body.ToString(),
                    //    Subject = "Reset Password"
                    //});
                    await mailService.SendEmailAsync(new MailRequestViewModel()
                    {
                        Attachments = null,
                        Body = body.ToString(),
                        Subject = "Email Confirmation Message",
                        ToEmail = model.Email
                    });
                }
                TempData["SuccessMessage"] = "If your email matched an existing account in our system ,you should recive email ";

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(VerifyPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    var isValid = await userManager.VerifyUserTokenAsync(existingUser, TokenOptions.DefaultProvider,
                        "ResetPassword", model.Token);

                    if (isValid)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Token is invalid";
                    }

                }
            }
            return View("Login");

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    var resultResetPassword = await userManager.ResetPasswordAsync(existingUser, model.Token, model.NewPassword);

                    if (resultResetPassword.Succeeded)
                    {
                        TempData["SuccessMessage"] = "your password successfully resetted";
                        return RedirectToAction("Login");
                    }
                    foreach (var error in resultResetPassword.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

    }
}
