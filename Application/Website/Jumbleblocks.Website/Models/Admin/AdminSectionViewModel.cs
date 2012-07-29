using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jumbleblocks.Website.Models.Admin
{
    /// <summary>
    /// represents a section on the admin screen
    /// </summary>
    public class AdminSectionViewModel
    {
        public AdminSectionViewModel()
        {
            Links = new List<AdminSectionLink>(0);
        }

        /// <summary>
        /// Name of section
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns links to display under admin section
        /// </summary>
        public List<AdminSectionLink> Links { get; set; }
    }
}