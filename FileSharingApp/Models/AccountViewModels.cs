using FileSharingApp.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name ="Email",ResourceType =typeof(Shared))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Password", ResourceType = typeof(Shared))]
        public string Password { get; set; }
    }
    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessageResourceName ="Email",ErrorMessageResourceType =typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Email", ResourceType = typeof(Shared))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Password", ResourceType = typeof(Shared))]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "ComparePassword", ResourceType =typeof(Shared))]
        public string ComparePassword { get; set; }
        [Display(Name = "FirstName",ResourceType =typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        public string LastName { get; set; }
    }

    public class ChangePasswordViewModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Shared))]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "NewPassword", ResourceType = typeof(Shared))]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        [Display(Name = "ConfirmedNewPassword", ResourceType = typeof(Shared))]
        public string ConfirmedNewPassword { get; set; }
    }
    public class AddPasswordViewModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        public string UserId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        public string Token { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Email", ResourceType = typeof(Shared))]
        public string Email { get; set; }
    }
    public class VerifyPasswordViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Email", ResourceType = typeof(Shared))]
        public string Email { get; set; }

        public string Token { get; set; }
    }
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        [Display(Name = "ConfirmedNewPassword", ResourceType = typeof(Shared))]
        public string ConfirmedNewPassword { get; set; }
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Shared))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Shared))]
        [Display(Name = "Email", ResourceType = typeof(Shared))]
        public string Email { get; set; }

        public string Token { get; set; }
    }

}
