using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models.Entities
{
    public class Publisher
    {
        public Publisher()
        {
            Translations = new List<PublisherTranslation>();
        }

        public long Id { get; set; }

        [MaxLength(40)]
        public string CompanyName { get; set; }

        public ICollection<PublisherTranslation> Translations { get; set; }

        [Column(TypeName = "ntext")]
        public string HomePage { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Game> Game { get; set; }
    }
}
