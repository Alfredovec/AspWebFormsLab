using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DALInfrastructure.RefModel;

namespace GameStore.GameStoreDAL.Model
{
    class DeletedModel
    {
        public long Id { get; set; }

        public long DeletedId { get; set; }

        public long? NewId { get; set; }

        public DatabaseName DatabaseName { get; set; }

        public TableName TableName { get; set; }
    }
}
