using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStorePublisherRepository : IPublisherRepository
    {
        private readonly GameStoreContext _db;
        private readonly IStoreOuterRefNavigators _refNavigators;

        public GameStorePublisherRepository(GameStoreContext context, IStoreOuterRefNavigators refNavigators)
        {
            _db = context;
            _refNavigators = refNavigators;
        }

        public void Create(Publisher item)
        {
            _db.Publishers.Add(item);
        }

        public void Edit(Publisher item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Publisher item)
        {
            item.IsDeleted = true;
            Edit(item);
        }

        public Publisher Get(long id)
        {
            var publisher = _db.Publishers.Find(id);
            _db.Entry(publisher).State = EntityState.Detached;
            return publisher;
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _db.Publishers.AsNoTracking().Include(g=>g.Translations).Where(p=>!p.IsDeleted).ToList();
            return publishers.Select(MapPublisher);
            }

        private Publisher MapPublisher(Publisher publisher)
        {
            publisher.Id = _refNavigators.PublisherRefNavigator.GenerateGlobalId(publisher.Id, DatabaseName.GameStore);
            return publisher;
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }
    }
}
