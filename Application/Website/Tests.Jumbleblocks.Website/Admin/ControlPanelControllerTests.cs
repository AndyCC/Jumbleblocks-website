using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using System.Web.Mvc;
using System.Dynamic;
using Jumbleblocks.Website.Models.Admin;
using Jumbleblocks.Website.Controllers.Admin;

namespace Tests.Jumbleblocks.Website.Admin
{
    [TestFixture]
    public class ControlPanelControllerTests
    {
        [Test]
        public void Index_Returns_ViewResult()
        {
            var controller = new ControlPanelController();
            var result = controller.Index();
            result.ShouldBeInstanceOfType(typeof(ViewResult));
        }

        [Test]
        public void Index_Returns_ControlPanel_View()
        {
            var controller = new ControlPanelController();
            var result = controller.Index();

            ((ViewResult)result).ViewName.ShouldEqual("ControlPanel");
        }

        [Test]
        public void Index_Returns_ViewModel_Of_ControlPanelViewModel()
        {
            var controller = new ControlPanelController();
            var result = controller.Index();

            ((ViewResult)result).Model.ShouldBeInstanceOfType(typeof(ControlPanelViewModel));
        }

        [Test]
        public void Index_Returns_ViewModel_With_AdminSectionViewModel_For_Section_Blog_Posts()
        {
            var controller = new ControlPanelController();
            var result = controller.Index() as ViewResult;

            ((ControlPanelViewModel)result.Model).Sections.SingleOrDefault(asvm => asvm.Name.Equals("Blog Posts")).ShouldNotBeNull();
        }

        [Test]
        public void Index_Returns_ViewModel_With_Link_To_Create_New()
        {
            var controller = new ControlPanelController();
            var result = controller.Index() as ViewResult;

            var controlPanelViewModel = ((ControlPanelViewModel)result.Model).Sections.Single(asvm => asvm.Name.Equals("Blog Posts"));

            var createNewLink = controlPanelViewModel.Links.SingleOrDefault(asl => asl.Text.Equals("Create New"));

            createNewLink.ControllerName.ShouldEqual("BlogPost");
            createNewLink.ActionName.ShouldEqual("CreateNew");
        }

        [Test]
        public void Index_Returns_ViewModel_With_Link_To_List_Published()
        {
            const string FilterParamName = "filter";

            var controller = new ControlPanelController();
            var result = controller.Index() as ViewResult;

            var controlPanelViewModel = ((ControlPanelViewModel)result.Model).Sections.Single(asvm => asvm.Name.Equals("Blog Posts"));

            var listLink = controlPanelViewModel.Links.SingleOrDefault(asl => asl.Text.Equals("List Published"));

            listLink.ControllerName.ShouldEqual("BlogPost");
            listLink.ActionName.ShouldEqual("List");

            ((IDictionary<string, object>)listLink.Parameters).ContainsKey(FilterParamName).ShouldBeTrue("Should have filter parameter");
            ((IDictionary<string, object>)listLink.Parameters)[FilterParamName].ShouldEqual("Published");
        }
    }
}
