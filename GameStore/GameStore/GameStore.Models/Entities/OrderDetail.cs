using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models.Entities
{
    public class OrderDetail
    {
        public long Id { get; set; }
        
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Game Game { get; set; }
        
        public short Quantity { get; set; }

        public decimal Discount { get; set; }

        public virtual Order Order { get; set; }
    }
}