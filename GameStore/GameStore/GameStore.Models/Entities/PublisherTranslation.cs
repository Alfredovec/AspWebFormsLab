using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public class PublisherTranslation
    {
        public const Language DefaultLanguage = Language.Ru;

        public long Id { get; set; }

        public Language Language { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }
    }
}
