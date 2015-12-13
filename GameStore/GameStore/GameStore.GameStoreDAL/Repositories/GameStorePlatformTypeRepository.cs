using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStorePlatformTypeRepository : IPlatformTypeRepository
    {
        private readonly GameStoreContext _db;

        public GameStorePlatformTypeRepository(GameStoreContext context)
        {
            _db = context;
        }

        public void Create(PlatformType item)
        {
            _db.PlatformTypes.Add(item);
        }

        public void Edit(PlatformType item)
        {
            _db.SetState(item, EntityState.Modified);
        }

        public void Delete(PlatformType item)
        {
            _db.PlatformTypes.Remove(item);
        }

        public PlatformType Get(long id)
        {
            return _db.PlatformTypes.Find(id);
        }

        public IEnumerable<PlatformType> Get()
        {
            return _db.PlatformTypes.ToList();
        }

        public IEnumerable<PlatformType> Get(Func<PlatformType, bool> predicate)
        {
            return _db.PlatformTypes.Where(predicate).ToList();
        }

        public IEnumerable<PlatformType> GetPlatformTypesForGame(long gameId)
        {
            return _db.PlatformTypes.Where(p => p.Games.Any(game => game.Id == gameId)).ToList();
        }
    }
}
