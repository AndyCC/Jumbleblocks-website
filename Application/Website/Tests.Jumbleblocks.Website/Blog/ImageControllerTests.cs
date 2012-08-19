using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using System.Web.Mvc;
using Moq;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Website.Controllers;
using Jumbleblocks.Website.Models.BlogPost;
using Jumbleblocks.Website.Controllers.Blog;
using Jumbleblocks.Core.Logging;

namespace Tests.Jumbleblocks.Website.Blog
{
    /// <summary>
    /// Tests for an image controller
    /// </summary>
    [TestClass]
    public class ImageControllerTests
    {
        private Mock<IImageReferenceRepository> GetMockedImageReferenceRepositoryWith4Images()
        {
            var imageReferenceA = new ImageReference(1, "~/BlogImages/a.png");
            var imageReferenceB = new ImageReference(2, "~/BlogImages/b.png");
            var imageReferenceC = new ImageReference(3, "~/BlogImages/c.png");
            var imageReferenceD = new ImageReference(4, "~/BlogImages/d.png");

            var imageReferenceArray =  new ImageReference[] 
            { 
                imageReferenceA,
                imageReferenceB, 
                imageReferenceC, 
                imageReferenceD
            };

            var mockedImageReferenceRepository = new Mock<IImageReferenceRepository>();

            mockedImageReferenceRepository.Setup(r => r.LoadRange(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => imageReferenceArray.Skip(skip).Take(take));

            return mockedImageReferenceRepository;
        }

       [TestMethod]
        public void ImageList_Returns_Json_Result()
        {
            var mockedImageReferenceRepository = GetMockedImageReferenceRepositoryWith4Images();

            var controller = new ImageController(mockedImageReferenceRepository.Object, new Mock<IJumbleblocksLogger>().Object);

            var result = controller.ImageList(0, 4);
            result.ShouldBeInstanceOfType(typeof(JsonResult));
        }

       [TestMethod]
        public void ImageList_Returns_List_Of_ImageList()
        {
            var mockedImageReferenceRepository = GetMockedImageReferenceRepositoryWith4Images();
            var controller = new ImageController(mockedImageReferenceRepository.Object, new Mock<IJumbleblocksLogger>().Object);

            var result = (JsonResult)controller.ImageList(0, 4);
            result.Data.ShouldBeInstanceOfType(typeof(ImageList));
        }

       [TestMethod]
        public void ImageList_Returns_ImageList_Of_ImageListItem_Representing_ImageReferences()
        {
            var mockedImageReferenceRepository = GetMockedImageReferenceRepositoryWith4Images();
            var imageReferenceRepository = mockedImageReferenceRepository.Object;

            var allImages = imageReferenceRepository.LoadRange(0, 4).ToArray();

            var controller = new ImageController(mockedImageReferenceRepository.Object, new Mock<IJumbleblocksLogger>().Object);

            var result = (JsonResult)controller.ImageList(0, 4);
            var imageListViewModel = (ImageList)result.Data;

            imageListViewModel.ImageViewModels.ShouldContain(new ImageListItem { Id = allImages[0].Id.Value, Url = allImages[0].Url }, "Does not contain expected image url '{0}'", allImages[0].Url);
            imageListViewModel.ImageViewModels.ShouldContain(new ImageListItem { Id = allImages[1].Id.Value, Url = allImages[1].Url }, "Does not contain expected image url '{0}'", allImages[1].Url);
            imageListViewModel.ImageViewModels.ShouldContain(new ImageListItem { Id = allImages[2].Id.Value, Url = allImages[2].Url }, "Does not contain expected image url '{0}'", allImages[2].Url);
            imageListViewModel.ImageViewModels.ShouldContain(new ImageListItem { Id = allImages[3].Id.Value, Url = allImages[3].Url }, "Does not contain expected image url '{0}'", allImages[3].Url);
        }
    }
}
