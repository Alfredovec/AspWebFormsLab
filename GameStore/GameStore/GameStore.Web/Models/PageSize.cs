using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public enum PageSize
    {
        [Display(ResourceType = typeof (GlobalRes), Name = "Ten")]
        Ten = 10,
        [Display(ResourceType = typeof (GlobalRes), Name = "Twenty")]
        Twenty = 20,
        [Display(ResourceType = typeof (GlobalRes), Name = "Fifty")]
        Fifty = 50,
        [Display(ResourceType = typeof (GlobalRes), Name = "OneHundred")]
        OneHundred = 100,
        [Display(ResourceType = typeof(GlobalRes), Name = "All")]
        All = 0
    }
}