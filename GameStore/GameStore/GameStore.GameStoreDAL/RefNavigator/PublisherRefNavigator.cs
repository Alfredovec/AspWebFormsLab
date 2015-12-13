using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL.RefNavigator
{
    class PublisherRefNavigator : BaseRefNavigator<Publisher>
    {
        public PublisherRefNavigator(GameStoreContext db)
            : base(db)
        {
        }

        public override IEnumerable<Publisher> SetGlobalRef(IEnumerable<Publisher> items, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(items.Select(p => p.Id), dbName, TableName.Publisher);
            var result = new List<Publisher>();
            foreach (var publisher in items)
            {
                publisher.Id = globalIds[publisher.Id];
                result.Add(publisher);
            }
            return result;
        }

        public override IEnumerable<Publisher> SetGlobalRefWithCheckDeletion(IEnumerable<Publisher> items, DatabaseName dbName, bool loadNewest = true)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(items.Select(p => p.Id), dbName, TableName.Publisher);
            var result = new List<Publisher>();
            foreach (var publisher in items)
            {
                var newId = globalIds[publisher.Id];
                if (newId.IsDeleted)
                {
                    if (!loadNewest)
                    {
                        continue;
                    }
                    if (newId.NewGlobalId == null)
                    {
                        continue;
                    }
                    var g = _db.Publishers.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                    _db.Entry(g).State = EntityState.Detached;
                    result.Add(SetGlobalRef(g, DatabaseName.GameStore));
                }
                else
                {
                    publisher.Id = newId.Id;
                    result.Add(publisher);
                }
            }
            return result;
        }

        public override Publisher SetGlobalRef(Publisher item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(new[] { item.Id }, dbName, TableName.Publisher);
            item.Id = globalIds[item.Id];
            return item;
        }

        public override Publisher SetGlobalRefWithCheckDeletion(Publisher item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(new[] { item.Id }, dbName, TableName.Publisher);
            var newId = globalIds[item.Id];
            if (newId.IsDeleted)
            {
                if (newId.NewGlobalId == null)
                {
                    return null;
                }
                var publisher = _db.Publishers.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                _db.Entry(publisher).State = EntityState.Detached;
                return SetGlobalRef(publisher, DatabaseName.GameStore);
            }
            item.Id = newId.Id;

            return item;
        }

        public override Dictionary<DatabaseName, List<Publisher>> SetOriginRef(IEnumerable<Publisher> items)
        {
            var result = new Dictionary<DatabaseName, List<Publisher>>();
            foreach (var publisher in items)
            {
                var originDb = SetOriginRef(publisher);
                if (result.ContainsKey(originDb))
                {
                    result[originDb].Add(publisher);
                }
            }
            return result;
        }

        public override DatabaseName SetOriginRef(Publisher item)
        {
            var navigator = DecodeGlobalId(item.Id);
            return navigator.DatabaseName;
        }
    }
}