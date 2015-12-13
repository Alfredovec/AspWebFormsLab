using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL.Model;

namespace GameStore.GameStoreDAL.RefNavigator
{
    abstract class BaseRefNavigator<T> : IRefNavigator<T>
    {
        protected readonly GameStoreContext _db;
        private int MAX_DB_COUNT = 47;

        protected BaseRefNavigator(GameStoreContext context)
        {
            _db = context;
        }

        public long GenerateGlobalId(long id, DatabaseName dbName)
        {
            return id * MAX_DB_COUNT + (int) dbName;
        }

        public RefNavigation DecodeGlobalId(long globalId)
        {
            return new RefNavigation
            {
                Id = globalId,
                DatabaseName = (DatabaseName)(globalId%MAX_DB_COUNT),
                OriginId = globalId/MAX_DB_COUNT
            };
        }

        protected Dictionary<long, long> MapGlobalIds(IEnumerable<long> ids, DatabaseName dbName, TableName tbName)
        {
            return ids.Distinct().ToDictionary(i => i, i => GenerateGlobalId(i, dbName));
        }

        protected Dictionary<long, RefNavigation> MapGlobalIdsWithCheckDeletion(IEnumerable<long> ids, DatabaseName dbName, TableName tbName)
        {
            return ids.Distinct().ToDictionary(i => i, i =>GetNavigation(GenerateGlobalId(i, dbName), tbName));
        }

        public RefNavigation GetNavigation(long globalId, TableName tableName)
        {
            var navigation  = DecodeGlobalId(globalId);
            navigation.TableName = tableName;
            var deletedModel = _db.DeletedModels
                .FirstOrDefault(d => d.DeletedId == globalId && d.TableName == tableName);
            if (deletedModel != null)
            {
                navigation.IsDeleted = true;
                navigation.NewGlobalId = deletedModel.NewId;
            }
            return navigation;
        }

        public void RemoveNavigation(long globalId, TableName tableName, long? newId = null)
        {
            var navigation = DecodeGlobalId(globalId);
            var deletedModel = new DeletedModel
            {
                TableName = tableName,
                DatabaseName = navigation.DatabaseName,
                DeletedId = globalId,
                NewId = newId
            };
            var prevNavigator =
                _db.DeletedModels.FirstOrDefault(d => d.DeletedId == globalId && d.TableName == tableName);
            if (prevNavigator != null)
            {
                _db.DeletedModels.Remove(prevNavigator);
            }
            _db.DeletedModels.Add(deletedModel);
        }

        public Dictionary<DatabaseName, List<long>> SetOriginRef(IEnumerable<long> ids)
        {
            var result = new Dictionary<DatabaseName, List<long>>();
            foreach (var id in ids)
            {
                var originDb = DecodeGlobalId(id);
                if (!result.ContainsKey(originDb.DatabaseName))
                {
                    result[originDb.DatabaseName] = new List<long>();
                }
                result[originDb.DatabaseName].Add(originDb.OriginId);
            }
            return result;
        }

        public abstract IEnumerable<T> SetGlobalRef(IEnumerable<T> items, DatabaseName dbName);

        public abstract IEnumerable<T> SetGlobalRefWithCheckDeletion(IEnumerable<T> items, DatabaseName dbName,
            bool loadNewest = true);

        public abstract T SetGlobalRef(T item, DatabaseName dbName);

        public abstract T SetGlobalRefWithCheckDeletion(T item, DatabaseName dbName);

        public abstract Dictionary<DatabaseName, List<T>> SetOriginRef(IEnumerable<T> items);

        public abstract DatabaseName SetOriginRef(T item);


        public IEnumerable<T> SetGlobalRefWithCheckDeletion(IEnumerable<T> items, DatabaseName dbName)
        {
            throw new NotImplementedException();
        }
    }
}