using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.BlogPost
{
    [Serializable]
    public class ImageList //: IEnumerable<ImageViewModel>
    {
        public ImageList(IEnumerable<ImageListItem> imageViewModels, int totalCount)
        {
            if (imageViewModels == null)
                ImageViewModels = new List<ImageListItem>();
            else
                ImageViewModels = imageViewModels;

            TotalCount = totalCount;
        }

        public IEnumerable<ImageListItem> ImageViewModels { get; private set; }

        public int ReturnedCount { get { return ImageViewModels.Count(); } }
        public int TotalCount { get; private set; }

    }
}