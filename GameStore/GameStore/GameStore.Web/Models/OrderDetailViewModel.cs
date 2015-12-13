using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class OrderDetailViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Game")]
        public GameViewModel Game { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Quantity")]
        public short Quantity { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Discount")]
        public double Discount { get; set; }
    }
}