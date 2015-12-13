using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class DateDiaposonViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(GlobalRes), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GlobalRes))]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(GlobalRes), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}