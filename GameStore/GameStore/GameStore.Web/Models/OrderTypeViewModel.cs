using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public enum OrderTypeViewModel
    {
        [Display(ResourceType = typeof (GlobalRes), Name = "MostViewed")]
        MostViewed,
        [Display(ResourceType = typeof (GlobalRes), Name = "MostCommented")]
        MostCommented,
        [Display(ResourceType = typeof (GlobalRes), Name = "ByPriceAsc")]
        ByPriceAsc,
        [Display(ResourceType = typeof (GlobalRes), Name = "ByPriceDesc")]
        ByPriceDesc,
        [Display(ResourceType = typeof (GlobalRes), Name = "NewFirst")]
        New,
    }
}