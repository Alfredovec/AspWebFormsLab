//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameStore.NorthwindDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Territories
    {
        public Territories()
        {
            this.Employees = new HashSet<Employees>();
        }
    
        public string TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }
    
        public virtual Region Region { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
