using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Jumbleblocks.Website.Models.BlogPost
{
    public class CreateEditModel
    {
        [Display(Name = "Id")]
        public int? BlogPostId { get; set; }

        [Display(Name="Date Published")]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy hh:mm", ApplyFormatInEditMode = true)]
        public DateTime? PublishedDate { get; set; }

        [Required]
        [Display(Name= "Title")]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Front Page Description")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Full Article")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullArticle { get; set; }

        [Display(Name = "Tags")]
        [DataType(DataType.Text)]
        public string TagTexts { get; set; } //Only for db atm

        [Display(Name="Series")]
        [DataType(DataType.Text)]
        public string SeriesName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? ImageId { get; set; }
    }
}