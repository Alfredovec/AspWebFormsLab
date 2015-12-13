using System.Collections.Generic;

namespace GameStore.Web.Models
{
    public class GenreViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<GenreViewModel> Children { get; set; }
    }
}