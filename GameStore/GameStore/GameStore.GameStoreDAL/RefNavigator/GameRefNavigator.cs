using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL.RefNavigator
{
    class GameRefNavigator : BaseRefNavigator<Game>
    {
        public GameRefNavigator(GameStoreContext db)
            : base(db)
        {
        }

        public override IEnumerable<Game> SetGlobalRef(IEnumerable<Game> items, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(items.Select(p => p.Id), dbName, TableName.Game);
            var result = new List<Game>();
            foreach (var game in items)
            {
                game.Id = globalIds[game.Id];
                result.Add(game);
            }
            return result;
        }

        public override IEnumerable<Game> SetGlobalRefWithCheckDeletion(IEnumerable<Game> items, DatabaseName dbName, bool loadNewest = true)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(items.Select(p => p.Id), dbName, TableName.Game);
            var result = new List<Game>();
            foreach (var game in items)
            {
                var newId = globalIds[game.Id];
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
                    var g = _db.Games.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                    _db.Entry(g).State = EntityState.Detached;
                    result.Add(SetGlobalRef(g, DatabaseName.GameStore));
                }
                else
                {
                    game.Id = newId.Id;
                    result.Add(game);
                }
            }
            return result;
        }

        public override Game SetGlobalRef(Game item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(new[] { item.Id }, dbName, TableName.Game);
            item.Id = globalIds[item.Id];
            return item;
        }

        public override Game SetGlobalRefWithCheckDeletion(Game item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(new[] { item.Id }, dbName, TableName.Game);
            var newId = globalIds[item.Id];
            if (newId.IsDeleted)
            {
                if (newId.NewGlobalId != null)
                {
                    var game = _db.Games.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                    _db.Entry(game).State = EntityState.Detached;
                    return SetGlobalRef(game, DatabaseName.GameStore);
                }
            }
                item.Id = newId.Id;
                
            return item;
        }

        public override Dictionary<DatabaseName, List<Game>> SetOriginRef(IEnumerable<Game> items)
        {
            var result = new Dictionary<DatabaseName, List<Game>>();
            foreach (var game in items)
            {
                var originDb = SetOriginRef(game);
                if (result.ContainsKey(originDb))
                {
                    result[originDb].Add(game);
                }
            }
            return result;
        }

        public override DatabaseName SetOriginRef(Game item)
        {
            var navigator = DecodeGlobalId(item.Id);
            return navigator.DatabaseName;
        }
    }
}
