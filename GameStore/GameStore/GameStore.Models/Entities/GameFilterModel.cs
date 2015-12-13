using System.Collections.Generic;
using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class GameFilterModel
    {
        public IEnumerable<long> GenreNames { get; set; }

        public IEnumerable<long> PlatformTypesNames { get; set; }

        public IEnumerable<long> PublishersName { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public string Name { get; set; }

        public DateFilter DateFilter { get; set; }

        public OrderType OrderType { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
