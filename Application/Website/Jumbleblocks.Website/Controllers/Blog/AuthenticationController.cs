using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Web.Core;
using Jumbleblocks.Web.Security;
using Jumbleblocks.Website.Models.Authentication;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Website.ActionFilters;

namespace Jumbleblocks.Website.Controllers.Blog
{
    /// <summary>
    /// Controls authentication on the website
    /// </summary>
    [NoCache]
    [HandleAndLogErrorAttribute(ExceptionType = typeof(Exception), View = "ErrorOccured")]
    public class AuthenticationController : Controller
    {
        public AuthenticationController(IWebAuthenticator webAuthenticator, IConfigurationReader configurationReader)
        {
            if (webAuthenticator == null)
                throw new ArgumentNullException("webAuthenticator");

            if (configurationReader == null)
                throw new ArgumentNullException("configurationReader");

            WebAuthenticator = webAuthenticator;
            ConfigurationReader = configurationReader;
        }

        public IWebAuthenticator WebAuthenticator { get; private set; }
        public IConfigurationReader ConfigurationReader { get; private set; }

        protected BlogConfigurationSection BlogSettings
        {
            get { return ConfigurationReader.GetSection<BlogConfigurationSection>(BlogContants.BlogSettingsConfigName); }
        }

        /// <summary>
        /// Gets login form
        /// </summary>
        /// <param name="redirectUrl">Url to redirect to after successful login</param>
        /// <returns>ActionResult</returns>
        public ActionResult LoginForm(string redirectUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ErrorMessage = "You do not have sufficient security rights";
                return View("Login");
            }

            var viewModel = new LoginViewModel();

            viewModel.Username = WebAuthenticator.GetUsernameFromCookie();
            viewModel.RedirectUrl = DetermineRedirectUrl(redirectUrl);
            
            ViewData.Model = viewModel;

            return View("Login");
        }

        //TODO: extract out into helper class
        private string DetermineRedirectUrl(string redirectUrl)
        {
            if(String.IsNullOrWhiteSpace(redirectUrl))
                return BlogSettings.AuthenticationDefaultRedirectUrl;

            if (redirectUrl.StartsWith("/"))
                redirectUrl = "~" + redirectUrl;

            if (redirectUrl.StartsWith("~"))
                return redirectUrl;

            if (BlogSettings.UrlStartWithAllowedRedirectUrl(redirectUrl))
                return redirectUrl;
            else
                return BlogSettings.AuthenticationDefaultRedirectUrl;
        }

        /// <summary>
        /// Attempts to log user in
        /// </summary>
        /// <param name="viewModel">view model of login data</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Valid data not supplied";
                    return View("Login");
                }

                var principal = WebAuthenticator.Authenticate(viewModel.Username, viewModel.Password);

                if (principal.Identity.IsAuthenticated)
                {
                    string redirectUrl = DetermineRedirectUrl(viewModel.RedirectUrl);
                    return Redirect(redirectUrl);
                }

                ViewBag.ErrorMessage = "Invalid username, password or could not be validated.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View("Login");
        }

        /// <summary>
        /// Logs user out
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LogOut()
        {
            WebAuthenticator.LogOut();
            
            return RedirectToAction(BlogSettings.DefaultAction, BlogSettings.DefaultController);
        }
    }
}