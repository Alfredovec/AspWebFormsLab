using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class ShipperViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Phone")]
        public string Phone { get; set; }
    }
}