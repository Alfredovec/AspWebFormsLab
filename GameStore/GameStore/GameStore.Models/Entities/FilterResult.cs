using System.Collections.Generic;

namespace GameStore.Models.Entities
{
    public class FilterResult
    {
        public IEnumerable<Game> Games { get; set; }

        public int TotalPageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
