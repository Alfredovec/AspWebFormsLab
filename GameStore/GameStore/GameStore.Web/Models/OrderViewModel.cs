using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "CreationDate")]
        public DateTime Date { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Customer")]
        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "ShippedDate")]
        public DateTime? ShippedDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "OrderStatus")]
        [UIHint("Enum")]
        public OrderStatus OrderStatus { get;set;}

        [Display(ResourceType = typeof(GlobalRes), Name = "OrderDetails")]
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; } 
    }
}