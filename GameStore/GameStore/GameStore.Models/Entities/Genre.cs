using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GameStore.Models.Entities
{
    public class Genre
    {
        public Genre()
        {
            Translations = new List<GenreTranslation>();
        }

        public long Id { get; set; }
        
        public ICollection<GenreTranslation> Translations { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Genre Parent { get; set; }

        public ICollection<Genre> Children { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public override string ToString()
        {
            return string.Format("Genre: [Id:{0}]", Id);
        }
    }
}
