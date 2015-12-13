using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public class GameTranslation
    {
        public const Language DefaultLanguage = Language.Ru;

        public long Id { get; set; }

        public Language Language { get; set; }

        public string Description { get; set; }
    }
}
