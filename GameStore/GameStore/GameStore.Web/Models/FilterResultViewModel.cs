using System.Collections.Generic;

namespace GameStore.Web.Models
{
    public class FilterResultViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }

        public int TotalPageSize { get; set; }

        public int PageNumber { get; set; }

        public List<int> AwaiblePages { get; set; }
    }
}