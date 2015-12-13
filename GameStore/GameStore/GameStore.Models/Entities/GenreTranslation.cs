using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public class GenreTranslation
    {
        public const Language DefaultLanguage = Language.Ru;

        public long Id { get; set; }

        public Language Language { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
