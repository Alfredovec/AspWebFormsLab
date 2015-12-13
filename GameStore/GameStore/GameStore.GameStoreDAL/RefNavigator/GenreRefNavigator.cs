using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL.RefNavigator
{
    class GenreRefNavigator : BaseRefNavigator<Genre>
    {
        public GenreRefNavigator(GameStoreContext db) : base(db) { }

        public override IEnumerable<Genre> SetGlobalRef(IEnumerable<Genre> items, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(items.Select(p => p.Id), dbName, TableName.Genre);
            var result = new List<Genre>();
            foreach (var genre in items)
            {
                genre.Id = globalIds[genre.Id];
                result.Add(genre);
            }
            return result;
        }

        public override IEnumerable<Genre> SetGlobalRefWithCheckDeletion(IEnumerable<Genre> items, DatabaseName dbName, bool loadNewest = true)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(items.Select(p => p.Id), dbName, TableName.Genre);
            var result = new List<Genre>();
            foreach (var genre in items)
            {
                var newId = globalIds[genre.Id];
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
                    var g = _db.Genres.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                    _db.Entry(g).State = EntityState.Detached;
                    result.Add(SetGlobalRef(g, DatabaseName.GameStore));
                }
                else
                {
                    genre.Id = newId.Id;
                    result.Add(genre);
                }
            }
            return result;
        }

        public override Genre SetGlobalRef(Genre item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(new[] { item.Id }, dbName, TableName.Genre);
            item.Id = globalIds[item.Id];
            return item;
        }

        public override Genre SetGlobalRefWithCheckDeletion(Genre item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(new[] { item.Id }, dbName, TableName.Genre);
            var newId = globalIds[item.Id];
            if (newId.IsDeleted)
            {
                if (newId.NewGlobalId == null)
                {
                    return null;
                }
                var genre = _db.Genres.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                _db.Entry(genre).State = EntityState.Detached;
                return SetGlobalRef(genre, DatabaseName.GameStore);
            }
            item.Id = newId.Id;

            return item;
        }

        public override Dictionary<DatabaseName, List<Genre>> SetOriginRef(IEnumerable<Genre> items)
        {
            var result = new Dictionary<DatabaseName, List<Genre>>();
            foreach (var genre in items)
            {
                var originDb = SetOriginRef(genre);
                if (result.ContainsKey(originDb))
                {
                    result[originDb].Add(genre);
                }
            }
            return result;
        }

        public override DatabaseName SetOriginRef(Genre item)
        {
            var navigator = DecodeGlobalId(item.Id);
            return navigator.DatabaseName;
        }
    }
}
