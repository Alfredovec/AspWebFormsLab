using System.ComponentModel.DataAnnotations;
using GameStore.Models.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class PaymentViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "GameName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        public string Description { get; set; }

       [Display(ResourceType = typeof(GlobalRes), Name = "Icon")]
        public string ImagePath { get { return Type.ToString() + ".jpg"; } }

        [Display(ResourceType = typeof(GlobalRes), Name = "PaymentType")]
        public PaymentType Type { get; set; }
    }
}