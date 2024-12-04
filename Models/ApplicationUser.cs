using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EcomApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public override string Email 
        {
            get => base.Email;
            set => base.Email = value?.ToLower(); // Ensure email is stored in lowercase
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User name")]
        public override string UserName 
        {
            get => base.UserName;
            set => base.UserName = value?.ToLower(); // Ensure username is stored in lowercase
        }
        
       
    }
}