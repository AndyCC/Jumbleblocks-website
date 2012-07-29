using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.Admin
{
    /// <summary>
    /// View model for control panel
    /// </summary>
    public class ControlPanelViewModel
    {
        public ControlPanelViewModel()
        {
            Sections = new List<AdminSectionViewModel>(0);
        }

        /// <summary>
        /// Gets sections in the view model
        /// </summary>
        public List<AdminSectionViewModel> Sections { get; set; }
    }
}