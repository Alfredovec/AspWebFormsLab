using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class GameFilterViewModel
    {
        public IEnumerable<long> GenreNames { get; set; }

        public IEnumerable<long> PlatformTypesNames { get; set; }

        public IEnumerable<long> PublishersName { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Genres")]
        public List<GenreViewModel> Genres { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypes")]
        public List<PlatformTypeViewModel> PlatformTypes { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Publishers")]
        public List<PublisherViewModel> Publishers { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [Display(ResourceType = typeof(GlobalRes), Name = "MaxPrice")]
        public decimal MaxPrice { get; set; }

        [RegularExpression("([\\s\\S]{3,})?", ErrorMessageResourceType = typeof (GlobalRes), ErrorMessageResourceName = "GameFilterViewModel_NameError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "GameName")]
        public string Name { get; set; }

        [UIHint("EnumRadioButton")]
        [Display(ResourceType = typeof(GlobalRes), Name = "DateFilterName")]
        public DateFilterViewModel DateFilter { get; set; }

        [UIHint("Enum")]
        [Display(ResourceType = typeof(GlobalRes), Name = "OrderTitle")]
        public OrderTypeViewModel OrderType { get; set; }

        [UIHint("Enum")]
        [Display(ResourceType = typeof(GlobalRes), Name = "PageSize")]
        public PageSize PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}