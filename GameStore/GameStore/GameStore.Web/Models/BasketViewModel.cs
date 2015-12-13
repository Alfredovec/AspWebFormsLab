using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models
{
    public class BasketViewModel
    {
        public decimal Sum { get; set; }

        public int Count { get; set; }
    }
}