using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jumbleblocks.Website.Models.Authentication
{
    /// <summary>
    /// view model for logging in a user
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Username
        /// </summary>
        [Display(Name= "Username")]
        [Required(ErrorMessage="Username required")]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        [Required(ErrorMessage= "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Url to redirect to on successful login
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}