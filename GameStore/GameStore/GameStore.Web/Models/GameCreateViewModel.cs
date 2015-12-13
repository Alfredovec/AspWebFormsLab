using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class GameCreateViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Key")]
        public string Key { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "GameName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "DescriptionRu")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "DescriptionEn")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "DateValidationError")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(GlobalRes), Name = "PublishedDate")]
        public DateTime PublishedDate { get; set; }

        public long? PublisherId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Genres")]
        public List<long> Genres { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypes")]
        public List<long> PlatformTypes { get; set; }

        public List<GenreViewModel> AllGenres { get; set; }

        public List<PlatformTypeViewModel> AllPlatforms { get; set; }

        public List<PublisherViewModel> AllPublishers { get; set; }
    }
}