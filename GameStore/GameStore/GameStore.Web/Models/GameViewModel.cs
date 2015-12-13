using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class GameViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Key")]
        public string Key { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "GameName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Price")]
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Publisher")]
        public string PublisherName { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(GlobalRes), Name = "PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "CreationDate")]
        public DateTime CreationDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "ViewedCount")]
        public long ViewedCount { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Genres")]
        public ICollection<string> Genres { get; set; } 

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypes")]
        public ICollection<string> PlatformTypes { get; set; } 
    }
}