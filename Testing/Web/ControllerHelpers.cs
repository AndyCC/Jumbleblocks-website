using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Moq;

namespace Jumbleblocks.Testing.Web
{
    /// <summary>
    /// Test Helper class for mocked controllers
    /// </summary>
    public static class ControllerHelpers
    {
        /// <summary>
        /// Sets the mocked controller context
        /// </summary>
        /// <param name="controller">controller to mock context of</param>
        /// <exception cref="ArgumentNullException">controller is null</exception>
        public static void SetMockedControllerContext(this Controller controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            var httpContextBase = HttpContextHelper.CreateMockedHttpContextBase();

            var mockedControllerContext = new Mock<ControllerContext>();
            mockedControllerContext.SetupGet(ctx => ctx.HttpContext).Returns(httpContextBase);
            mockedControllerContext.SetupGet(ctx => ctx.RouteData).Returns(new RouteData());
            mockedControllerContext.SetupGet(ctx => ctx.Controller).Returns(controller);

            controller.ControllerContext = mockedControllerContext.Object;
        }

        /// <summary>
        /// Sets auth cookie on given controller
        /// </summary>
        /// <param name="controller">controller to have auth cookie</param>
        /// <param name="name">name to set in auth cookie</param>
        /// <param name="userData">any user data in auth cookie</param>
        public static void SetAuthCookie(this Controller controller, string name, string userData)
        {
            var ticket = new FormsAuthenticationTicket(1,
                                                       name,
                                                       DateTime.Now,
                                                       DateTime.Now.AddMinutes(30),
                                                       false,
                                                       userData,
                                                       FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            controller.Request.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
        }

        /// <summary>
        /// Performs data annotation validation on provided object and adds model state errors to controller
        /// </summary>
        /// <typeparam name="TObj">Object to validate</typeparam>
        /// <param name="controller">controller to add errors to</param>
        /// <param name="objectToValidate">object to validate</param>
        public static void DoDataAnnotationValidation<TObj>(this Controller controller, TObj objectToValidate)
        {
            var validationContext = new ValidationContext(objectToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(objectToValidate, validationContext, validationResults);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        /// <summary>
        /// Sets principal as current user on controller
        /// </summary>
        /// <param name="controller">controller set principal on</param>
        /// <param name="principal">principal to set</param>
        public static void SetPrincipal(this Controller controller, IPrincipal principal)
        {
            if (controller.ControllerContext == null)
                controller.SetMockedControllerContext();

            var mockedHttpContext = Mock.Get<HttpContextBase>(controller.HttpContext);
            mockedHttpContext.SetupGet(ctx => ctx.User).Returns(principal);
        }
    }
}
