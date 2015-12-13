using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Entities
{
    public class PlatformType
    {
        public long Id { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public override string ToString()
        {
            return string.Format("PlatformType: [Type:{0}]", Type);
        }
    }
}
