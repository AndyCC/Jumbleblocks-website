using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jumbleblocks.Website.Models.BlogPost;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Core.Logging;

namespace Jumbleblocks.Website.Controllers.Blog
{
    /// <summary>
    /// Controller for images
    /// </summary>
    public class ImageController : Controller
    {
        public ImageController(IImageReferenceRepository imageReferenceRepository, IJumbleblocksLogger logger)
        {
            if (imageReferenceRepository == null)
                throw new ArgumentNullException("imageReferenceRepository");

            if (logger == null)
                throw new ArgumentNullException("logger");

            ImageReferenceRepository = imageReferenceRepository;
            Logger = logger;
        }

        public IImageReferenceRepository ImageReferenceRepository { get; private set; }
        public IJumbleblocksLogger Logger { get; private set; }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ImageList(int skip, int take)
        {
            try
            {
                IEnumerable<ImageReference> imageReferences = ImageReferenceRepository.LoadRange(skip, take);
                IEnumerable<ImageListItem> imageViewModels = imageReferences.Select(ir => new ImageListItem { Id = ir.Id.Value, Url = ir.Url });

                int totalImageCount = ImageReferenceRepository.Count;
                var imageList = new ImageList(imageViewModels, totalImageCount);

                return Json(imageList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //TODO : better way to handle errors??
                Logger.LogError("ImageController - ImageList", ex);
                return Json(new ImageList[0], JsonRequestBehavior.AllowGet);
            }
        }

    }
}
