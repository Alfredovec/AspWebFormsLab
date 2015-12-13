using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public enum DateFilterViewModel
    {
        [Display(ResourceType = typeof (GlobalRes), Name = "DateFilterViewModel_All")]
        All,
        [Display(ResourceType = typeof (GlobalRes), Name = "LastWeek")]
        LastWeek,
        [Display(ResourceType = typeof (GlobalRes), Name = "LastMonth")]
        LastMonth,
        [Display(ResourceType = typeof (GlobalRes), Name = "LastYear")]
        LastYear,
        [Display(ResourceType = typeof (GlobalRes), Name = "TwoYear")]
        TwoYear,
        [Display(ResourceType = typeof (GlobalRes), Name = "ThreeYear")]
        ThreeYear
    }
}