using System.ComponentModel.DataAnnotations;

namespace GameStore.DALInfrastructure.RefModel
{
    public class RefNavigation
    {
        [Key]
        public long Id { get; set; }

        public DatabaseName DatabaseName { get; set; }

        public TableName TableName { get; set; }

        public long OriginId { get; set; }

        public bool IsDeleted { get; set; }

        public long? NewGlobalId { get; set; }
    }
}
