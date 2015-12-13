using System.Collections.Generic;
using GameStore.DALInfrastructure.RefModel;

namespace GameStore.DALInfrastructure.Interfaces
{
    public interface IRefNavigator<T>
    {
        IEnumerable<T> SetGlobalRef(IEnumerable<T> items, DatabaseName dbName);

        IEnumerable<T> SetGlobalRefWithCheckDeletion(IEnumerable<T> items, DatabaseName dbName,
            bool loadNewest = true);

        T SetGlobalRef(T item, DatabaseName dbName);

        T SetGlobalRefWithCheckDeletion(T item, DatabaseName dbName);

        Dictionary<DatabaseName, List<T>> SetOriginRef(IEnumerable<T> items);

        Dictionary<DatabaseName, List<long>> SetOriginRef(IEnumerable<long> ids);

        DatabaseName SetOriginRef(T item);

        RefNavigation GetNavigation(long globalId, TableName tableName);

        void RemoveNavigation(long globalId, TableName tableName, long? newId = null);

        long GenerateGlobalId(long id, DatabaseName dbName);

        RefNavigation DecodeGlobalId(long globalId);
    }
}