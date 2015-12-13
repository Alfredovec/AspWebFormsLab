using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL.RefNavigator
{
    class OrderRefNavigator : BaseRefNavigator<Order>
    {
        public OrderRefNavigator(GameStoreContext db)
            : base(db)
        {
        }

        public override IEnumerable<Order> SetGlobalRef(IEnumerable<Order> items, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(items.Select(p => p.Id), dbName, TableName.Order);
            var result = new List<Order>();
            foreach (var order in items)
            {
                order.Id = globalIds[order.Id];
                result.Add(order);
            }
            return result;
        }

        public override IEnumerable<Order> SetGlobalRefWithCheckDeletion(IEnumerable<Order> items, DatabaseName dbName, bool loadNewest = true)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(items.Select(p => p.Id), dbName, TableName.Order);
            var result = new List<Order>();
            foreach (var order in items)
            {
                var newId = globalIds[order.Id];
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
                    var g = _db.Orders.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                    _db.Entry(g).State = EntityState.Detached;
                    result.Add(SetGlobalRef(g, DatabaseName.GameStore));
                }
                else
                {
                    order.Id = newId.Id;
                result.Add(order);
            }
            }
            return result;
        }

        public override Order SetGlobalRef(Order item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIds(new[] { item.Id }, dbName, TableName.Order);
            item.Id = globalIds[item.Id];
            return item;
        }

        public override Order SetGlobalRefWithCheckDeletion(Order item, DatabaseName dbName)
        {
            var globalIds = MapGlobalIdsWithCheckDeletion(new[] { item.Id }, dbName, TableName.Order);
            var newId = globalIds[item.Id];
            if (newId.IsDeleted)
            {
                if (newId.NewGlobalId == null)
            {
                return null;
            }
                var order = _db.Orders.Find(DecodeGlobalId(newId.NewGlobalId.Value).OriginId);
                _db.Entry(order).State = EntityState.Detached;
                return SetGlobalRef(order, DatabaseName.GameStore);
            }
            item.Id = newId.Id;

            return item;
        }

        public override Dictionary<DatabaseName, List<Order>> SetOriginRef(IEnumerable<Order> items)
        {
            var result = new Dictionary<DatabaseName, List<Order>>();
            foreach (var order in items)
            {
                var originDb = SetOriginRef(order);
                if (result.ContainsKey(originDb))
                {
                    result[originDb].Add(order);
                }
            }
            return result;
        }

        public override DatabaseName SetOriginRef(Order item)
        {
            var navigator = DecodeGlobalId(item.Id);
            return navigator.DatabaseName;
        }
    }
}