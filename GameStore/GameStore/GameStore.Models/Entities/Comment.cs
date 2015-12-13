using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GameStore.Models.Entities
{
    public class Comment
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string Body { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Comment Parent { get; set; }

        public virtual ICollection<Comment> Children { get; set; }

        public virtual Comment Quote { get; set; }

        public virtual ICollection<Comment> Quotes { get; set; } 

        public long GameId { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }

        public override string ToString()
        {
            return string.Format("Comment:[Id:{0},Name:{1},Body:{2},Childrens:{3},Parent:{4},GameId:{5}]",
                Id, Name, Body, 
                Children==null?"":Children.Aggregate(new StringBuilder(), (s, i) => s.Append(i.Id)).ToString(), 
            Parent==null?"":Parent.Id.ToString(), GameId);
        }
    }
}
