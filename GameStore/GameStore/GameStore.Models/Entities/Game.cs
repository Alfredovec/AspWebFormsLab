using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GameStore.Models.Entities
{
    public class Game
    {
        public Game()
        {
            Comments = new List<Comment>();
            Genres = new List<Genre>();
            PlatformTypes = new List<PlatformType>();
            OrderDetails = new List<OrderDetail>();
            Translations = new List<GameTranslation>();
        }

        public long Id { get; set; }
        
        [MaxLength(50)]
        public string Key { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<GameTranslation> Translations { get; set; }

        public string ContentType { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime CreationDate { get; set; }

        public byte[] Picture { get; set; }

        public string ImgMimeType { get; set; }

        [NotMapped]
        public long ViewedCount { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<PlatformType> PlatformTypes { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } 

        public virtual Publisher Publisher { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Game:[Id:{0},Key:{1},Name:{2}]", Id, Key, Name);
        }
    }
}
