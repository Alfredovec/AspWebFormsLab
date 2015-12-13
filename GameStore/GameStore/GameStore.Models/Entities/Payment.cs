using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public PaymentType Type { get; set; }
    }
}
