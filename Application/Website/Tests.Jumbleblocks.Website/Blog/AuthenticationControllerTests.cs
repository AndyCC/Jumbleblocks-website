using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Web.Core;
using Jumbleblocks.Web.Security;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Moq;
using Jumbleblocks.Testing;
using Jumbleblocks.Testing.Web;
using System.Web.Mvc;
using Jumbleblocks.Core.Configuration;
using System.Linq.Expressions;
using Jumbleblocks.Core.Security;
using System.Security.Principal;
using Jumbleblocks.Website.Models.Authentication;
using Jumbleblocks.Website.Configuration;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Website.Controllers.Blog;

namespace Tests.Jumbleblocks.Website.Blog
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        #region Login Form

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_webAuthenticator_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var controller = new AuthenticationController(null, new Mock<IConfigurationReader>().Object);
        }
        

       [TestMethod]
        public void Ctor_WHEN_webAuthenticator_Is_Not_Null_THEN_Sets_WebAuthenticator_Property()
        {
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var expectedWebAuthenticator = mockedWebAuthenticator.Object;

            var controller = new AuthenticationController(expectedWebAuthenticator, new Mock<IConfigurationReader>().Object);
            controller.WebAuthenticator.ShouldEqual(expectedWebAuthenticator);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_configurationReader_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var controller = new AuthenticationController(new Mock<IWebAuthenticator>().Object, null);
        }


       [TestMethod]
        public void Ctor_WHEN_configurationReader_Is_Not_Null_THEN_Sets_ConfigurationReader_Property()
        {
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();

            var expectedConfigurationReader = mockedConfigurationReader.Object;

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, expectedConfigurationReader);
            controller.ConfigurationReader.ShouldEqual(expectedConfigurationReader);
        }

        private IJumbleblocksPrincipal GetPrincipal(bool isAuthenticated)
        {
            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.SetupGet(i => i.IsAuthenticated).Returns(isAuthenticated);

            var mockedPrincipal = new Mock<IJumbleblocksPrincipal>();
            mockedPrincipal.As<IPrincipal>().SetupGet(p => p.Identity).Returns(mockedIdentity.Object);
            mockedPrincipal.SetupGet(p => p.Identity).Returns(mockedIdentity.Object);

            return mockedPrincipal.Object;
        }
        
       [TestMethod]
        public void LoginForm_Returns_ViewResult()
        {
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(new BlogConfigurationSection());
            
            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm();

            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

       [TestMethod]
        public void LoginForm_Returns_Login_View_With_Login_ViewModel()
        {
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(new BlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm() as ViewResult;

            result.ViewName.ShouldEqual("Login");
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
        }

       [TestMethod]
        public void LoginForm_GIVEN_Username_ABC_Exists_In_Authorisation_Cookie_THEN_Returns_LoginView_With_Username_Filled_In()
        {
            const string username = "ABC";          
            
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(username);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(new BlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm() as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).Username.ShouldEqual(username);
        }

       [TestMethod]
        public void LoginForm_GIVEN_No_Username_In_Authorisation_Cookie_THEN_Returns_LoginViewModel_With_Username_Set_As_Empty_String()
        {
            const string username = "";

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(username);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(new BlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm() as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).Username.ShouldEqual(username);
        }

       [TestMethod]
        public void LoginForm_WHEN_RedirectUrl_PassedThrough_THEN_Sets_RedirectUrl_On_ViewModel()
        {
            const string url = "http://localhost/test.html";

            var blogConfiguration = new BlogConfigurationSection();

            var acceptedUrlConfig = new AcceptedRedirectUrlElement();
            acceptedUrlConfig.Url = "http://localhost/";

            blogConfiguration.AcceptedRedirectUrls = new AcceptedRedirectUrlCollection();
            blogConfiguration.AcceptedRedirectUrls.AuthenticationDefaultRedirectUrl = "http://jumbleblocks.com/";
            blogConfiguration.AcceptedRedirectUrls.CallMethod("BaseAdd", acceptedUrlConfig);

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(blogConfiguration);

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm(url) as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).RedirectUrl.ShouldEqual(url);
        }

       [TestMethod]
        public void LoginForm_GIVEN_Configuration_BlogConfigurationSection_Has_DefaultRedirectUrl_And_RedirectAcceptedHosts_Contains_localhost_WHEN_Redirect_Url_Is_For_A_Differnt_Web_Domain_THEN_Sets_RedirectUrl_To_DefaultRedirectUrl()
        {
            const string defaultRedirectUrl = "http://localhost/test.html";

            var blogConfiguration = new BlogConfigurationSection();

            var acceptedUrlConfig = new AcceptedRedirectUrlElement();
            acceptedUrlConfig.Url = "http://localhost/";

            blogConfiguration.AcceptedRedirectUrls = new AcceptedRedirectUrlCollection();
            blogConfiguration.AcceptedRedirectUrls.AuthenticationDefaultRedirectUrl = defaultRedirectUrl;
            blogConfiguration.AcceptedRedirectUrls.CallMethod("BaseAdd", acceptedUrlConfig);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>())).Returns(blogConfiguration);

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm("Http://www.bbc.co.uk") as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).RedirectUrl.ShouldEqual(defaultRedirectUrl);   
        }

       [TestMethod]
        public void LoginForm_GIVEN_No_redirectUrl_THEN_Sets_RedirectUrl_On_ViewModel_To_DefaultRedirectUrl_From_Config()
        {
            const string defaultRedirectUrl = "http://localhost/test.html";

            var blogConfiguration = new BlogConfigurationSection();
            
            blogConfiguration.AcceptedRedirectUrls = new AcceptedRedirectUrlCollection();
            blogConfiguration.AcceptedRedirectUrls.AuthenticationDefaultRedirectUrl = defaultRedirectUrl;

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>())).Returns(blogConfiguration);

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm() as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).RedirectUrl.ShouldEqual(defaultRedirectUrl); 
        }

       [TestMethod]
        public void LoginForm_GIVEN_redirectUrl_Starts_With_Slash_THEN_Adds_WigglyLine_To_Front_Of_Url_And_Returns_It_As_RedirectUrl()
        {
            const string url = "/admin";

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(c => c.GetUsernameFromCookie()).Returns(String.Empty);

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(new BlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.SetPrincipal(GetPrincipal(false));

            var result = controller.LoginForm(url) as ViewResult;

            result.ShouldNotBeNull();
            result.Model.ShouldBeInstanceOfType(typeof(LoginViewModel));
            ((LoginViewModel)result.Model).RedirectUrl.ShouldEqual("~"+url);
        }

        #endregion

        #region Login

        private BlogConfigurationSection GetBlogConfigurationSection(string acceptedUrl = "http://localhost/", string defaultRedirectUrl = "http://localhost/test.html", 
            string defaultController = "Blog", string defaultAction = "Index")
        {
            var blogConfiguration = new BlogConfigurationSection();
            blogConfiguration.DefaultAction = defaultAction;
            blogConfiguration.DefaultController = defaultController;

            var acceptedUrlConfig = new AcceptedRedirectUrlElement();
            acceptedUrlConfig.Url = acceptedUrl;

            blogConfiguration.AcceptedRedirectUrls = new AcceptedRedirectUrlCollection();
            blogConfiguration.AcceptedRedirectUrls.AuthenticationDefaultRedirectUrl = defaultRedirectUrl;
            blogConfiguration.AcceptedRedirectUrls.CallMethod("BaseAdd", acceptedUrlConfig);

            return blogConfiguration;
        }

    

       [TestMethod]
        public void Login_Calls_WebAuthenticator_With_Provided_Username_And_Password()
        {
            var viewModel = new LoginViewModel
            {
                Username = "username",
                Password = "psw",
                RedirectUrl = String.Empty
            };           

            Expression<Func<IWebAuthenticator, IJumbleblocksPrincipal>> verifiableAction = wa => wa.Authenticate(viewModel.Username, viewModel.Password);

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(verifiableAction).Returns(GetPrincipal(true)).Verifiable();

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);

            controller.Login(viewModel);

            mockedWebAuthenticator.Verify(verifiableAction, Times.Once());
        }

       [TestMethod]
        public void Login_WHEN_Username_Is_Empty_THEN_Returns_Login_View_With_ErrorMessage_On_Username()
        {
            var viewModel = new LoginViewModel
            {
                Username = String.Empty,
                Password = "psw",
                RedirectUrl = String.Empty
            };

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.DoDataAnnotationValidation(viewModel);

            var result = controller.Login(viewModel) as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("Login");

            result.ViewData.ModelState.IsValid.ShouldBeFalse();
            result.ViewData.ModelState.Count.ShouldEqual(1);
            result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage.ShouldEqual("Username required");
        }

       [TestMethod]
        public void Login_WHEN_Password_Is_Empty_THEN_Returns_Login_View_With_ErrorMessage_On_Password()
        {
            var viewModel = new LoginViewModel
            {
                Username = "username",
                Password = null,
                RedirectUrl = String.Empty
            };

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.DoDataAnnotationValidation(viewModel);

            var result = controller.Login(viewModel) as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("Login");

            result.ViewData.ModelState.IsValid.ShouldBeFalse();
            result.ViewData.ModelState.Count.ShouldEqual(1);
            result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage.ShouldEqual("Password required");
        } 


       [TestMethod]
        public void Login_WHEN_WebAuthenticator_Returns_Authenticated_User_THEN_Returns_RedirectUrl()
        {
            const string redirectUrl = "http://localhost/test.html";

            var viewModel = new LoginViewModel
            {
                Username = "username",
                Password = "pass",
                RedirectUrl = redirectUrl
            };

             var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(wa => wa.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(GetPrincipal(true));

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.DoDataAnnotationValidation(viewModel);

            var result = controller.Login(viewModel) as RedirectResult;

            result.ShouldNotBeNull();
            result.Url.ShouldEqual(redirectUrl);
        }

       [TestMethod]
        public void Login_GIVEN_RedirectUrl_Provided_Is_EmptyString_WHEN_WebAuthenticator_Returns_Authenticated_User_THEN_Returns_RedirectUrl_To_DefaultRedirectUrl()
        {
            const string redirectUrl = "http://localhost/testABC.html";

            var viewModel = new LoginViewModel
            {
                Username = "username",
                Password = "pass",
                RedirectUrl = String.Empty
            };

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(wa => wa.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(GetPrincipal(true));

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection(defaultRedirectUrl:redirectUrl));

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.DoDataAnnotationValidation(viewModel);

            var result = controller.Login(viewModel) as RedirectResult;

            result.ShouldNotBeNull();
            result.Url.ShouldEqual(redirectUrl);
        }

       [TestMethod]
        public void Login_WHEN_WebAuthenticator_Authenticate_Returns_Principal_That_Is_Not_Authenticated_THEN_Returns_LoginView_With_ErrorMessage()
        {
            var viewModel = new LoginViewModel
            {
                Username = "username",
                Password = "pass",
                RedirectUrl = String.Empty
            };

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(wa => wa.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetPrincipal(false));

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection());


            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            controller.DoDataAnnotationValidation(viewModel);

            var result = controller.Login(viewModel) as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("Login");

            string errorMessage = result.ViewBag.ErrorMessage;
            errorMessage.ShouldEqual("Invalid username, password or could not be validated.");
        }

        #endregion

        #region Logout

       [TestMethod]
        public void Logout_Calls_Logout_On_WebAuthenticator()
        {
            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            Expression<Action<IWebAuthenticator>> verifiableAction = wa => wa.LogOut();
            mockedWebAuthenticator.Setup(verifiableAction).Verifiable();

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
               .Returns(GetBlogConfigurationSection());

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);
            var result = controller.LogOut();

            mockedWebAuthenticator.Verify(verifiableAction, Times.Once());
        }

       [TestMethod]
        public void Logout_GIVEN_BlogConfiguration_DefaultAction_Is_Index_And_Default_Controller_Is_Blog_THEN_Redirects_To_Index_On_Blog()
        {
            const string defaultController = "Blog";
            const string defaultAction = "Action";

            var mockedWebAuthenticator = new Mock<IWebAuthenticator>();
            mockedWebAuthenticator.Setup(wa => wa.LogOut());

            var mockedConfigurationReader = new Mock<IConfigurationReader>();
            mockedConfigurationReader.Setup(cr => cr.GetSection<BlogConfigurationSection>(It.IsAny<string>()))
                .Returns(GetBlogConfigurationSection(defaultController: defaultController, defaultAction: defaultAction));

            var controller = new AuthenticationController(mockedWebAuthenticator.Object, mockedConfigurationReader.Object);

            var result = controller.LogOut() as RedirectToRouteResult;
           
            result.ShouldNotBeNull();
            result.RouteValues["controller"].ShouldEqual(defaultController);
            result.RouteValues["action"].ShouldEqual(defaultAction);
        }

        #endregion
    }
}
