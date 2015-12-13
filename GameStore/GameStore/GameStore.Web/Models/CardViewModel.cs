using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class CardViewModel
    {
        /*
         * Visa – should redirect to bank’s page. Just make Stub page with fields: 
         * cart holder’s name, card number, Date of expiry (month and year), CVV2/CVC2.
         */
        [Display(ResourceType = typeof(GlobalRes), Name = "VisaName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "CardName")]
        public string CardNumber { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "DateOfExpire")]
        [RegularExpression("((0[1-9])|(1[0-2]))/[0-9]{4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ExpireDateValidation")]
        public string DateOfExpire { get; set; }

        [Range(1000, 10000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RangeValidation")]
        [Display(ResourceType = typeof(GlobalRes), Name = "CVC")]
        public int Cvc { get; set; }

        public CardType CardType { get; set; }
    }
}