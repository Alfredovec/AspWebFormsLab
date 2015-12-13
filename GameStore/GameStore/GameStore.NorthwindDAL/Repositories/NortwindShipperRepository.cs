using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Models.Repositories;

namespace GameStore.NorthwindDAL.Repositories
{
    class NorthWindShipperRepository : IShipperRepository
    {
        private readonly Entities _entities;

        public NorthWindShipperRepository(Entities entities)
        {
            _entities = entities;
        }

        public void Create(Models.Entities.Shipper item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Models.Entities.Shipper item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Models.Entities.Shipper item)
        {
            throw new NotImplementedException();
        }

        public Models.Entities.Shipper Get(long id)
        {
            return Mapper.Map<Models.Entities.Shipper>(_entities.Shippers.Find(id));
        }

        public IEnumerable<Models.Entities.Shipper> Get()
        {
            return _entities.Shippers.Select(Mapper.Map<Models.Entities.Shipper>).ToList();
        }

        public IEnumerable<Models.Entities.Shipper> Get(Func<Models.Entities.Shipper, bool> predicate)
        {
            return Get().Where(predicate);
        }
    }
}
