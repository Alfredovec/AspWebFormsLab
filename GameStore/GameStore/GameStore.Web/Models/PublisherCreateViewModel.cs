using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class PublisherCreateViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [MaxLength(40)]
        [RegularExpression("[^#?]+", ErrorMessageResourceType = typeof (GlobalRes), ErrorMessageResourceName = "CompanyNameValidationMessage")]
        [Display(ResourceType = typeof(GlobalRes), Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(GlobalRes), Name = "DescriptionEn")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(GlobalRes), Name = "DescriptionRu")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "UrlValidation")]
        [Display(ResourceType = typeof(GlobalRes), Name = "HomePage")]
        public string HomePage { get; set; }
    }
}