using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.NorthwindDAL.Repositories
{
    class NorthwinPublisherRepository : IPublisherRepository
    {
        private readonly Entities _entities;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public NorthwinPublisherRepository(Entities entities, IStoreOuterRefNavigators refNavigators)
        {
            _entities = entities;
            _refNavigators = refNavigators;
        }

        public void Create(Publisher item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Publisher item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Publisher item)
        {
            throw new NotImplementedException();
        }

        public Publisher Get(long id)
        {
            return Mapper.Map<Publisher>(_entities.Suppliers.Find(id));
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _entities.Suppliers.Select(Mapper.Map<Publisher>);
            return _refNavigators.PublisherRefNavigator.SetGlobalRefWithCheckDeletion(publishers, DatabaseName.Northwind, false);
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }
    }
}
