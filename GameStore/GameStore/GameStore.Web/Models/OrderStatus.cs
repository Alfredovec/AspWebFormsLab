using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public enum OrderStatus
    {
        [Display(ResourceType = typeof(GlobalRes), Name = "CreatedStatus")]
        Created,
        [Display(ResourceType = typeof(GlobalRes), Name = "PayedStatus")]
        Payed,
        [Display(ResourceType = typeof(GlobalRes), Name = "ShippedStatus")]
        Shipped
    }
}