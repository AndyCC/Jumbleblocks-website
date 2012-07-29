using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.BlogPost
{
    /// <summary>
    /// view model for a blog
    /// </summary>
    [Serializable]
    public class FrontPageModel
    {
        public FrontPageModel()
        {
            Paging = new PagingDetail();
        }

        /// <summary>
        /// Gets/sets the paging details 
        /// </summary>
        public PagingDetail Paging { get; set; }

        private List<FrontPageItemModel> _summaries = new List<FrontPageItemModel>(0);

        /// <summary>
        /// Gets/Sets list of summaries to show on view
        /// </summary>
        public IEnumerable<FrontPageItemModel> Summaries { get { return _summaries; } }

        /// <summary>
        /// Adds a BlogSummaryViewModel to the BlogViewModel
        /// </summary>
        /// <param name="summaryViewModel">summary view model</param>
        public void Add(FrontPageItemModel summaryViewModel)
        {
            if (summaryViewModel == null)
                throw new ArgumentNullException("summaryViewModel");

            _summaries.Add(summaryViewModel);
        }
    }
}