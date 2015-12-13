using System;
using System.Collections.Generic;
using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerId { get; set; }

        public DateTime? ShippedDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } 
    }
}
