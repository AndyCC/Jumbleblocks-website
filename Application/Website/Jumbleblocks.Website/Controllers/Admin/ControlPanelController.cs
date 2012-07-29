using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Website.Models.Admin;

namespace Jumbleblocks.Website.Controllers.Admin
{
    /// <summary>
    /// Controller to manage the control panel
    /// </summary>
    [HandleError(ExceptionType = typeof(Exception), View = "ErrorOccured")]
    public class ControlPanelController : Controller
    {
        //
        // GET: /ControlPanel/
        //

        [Authorize(Roles = "Blog Author")]
        public ActionResult Index()
        {
            var viewModel = new ControlPanelViewModel();

            var blogPostsSection = new AdminSectionViewModel { Name = "Blog Posts" };
            var link = new AdminSectionLink
                        {
                           Text = "Create New",
                           ControllerName = "BlogPost",
                           ActionName = "CreateNew"
                        };

            blogPostsSection.Links.Add(link);


            link = new AdminSectionLink
                      { 
                          Text = "List Published",
                          ControllerName = "BlogPost",
                          ActionName = "List"
                      };

            link.Parameters.filter = "Published";

            blogPostsSection.Links.Add(link); 

            viewModel.Sections.Add(blogPostsSection); //TODO: refactor

            ViewData.Model = viewModel;

            return View("ControlPanel");
        }

    }
}
