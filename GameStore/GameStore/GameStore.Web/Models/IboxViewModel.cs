using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class IboxViewModel
    {
        [Display(ResourceType = typeof(GlobalRes), Name = "OrderDetails")]
        public long OrderId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Customer")]
        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "SumOrder")]
        public decimal Sum { get; set; }
    }
}