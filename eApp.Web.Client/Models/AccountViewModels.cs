using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eApp.Web.Client.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        public string Region { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string mobile { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        public string username { get; set; }
        
        [Required]
        public string region { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string cPassword { get; set; }
        public string nPassword { get; set; }

    }
}

public enum Region
{
    JED,
    KHB,
    RYD
}
