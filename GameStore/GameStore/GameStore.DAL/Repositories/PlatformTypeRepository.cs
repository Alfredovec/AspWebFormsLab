using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class PlatformTypeRepository : IPlatformTypeRepository
    {
        private readonly IPlatformTypeRepository _mainPlatformTypeRepository;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public PlatformTypeRepository(IPlatformTypeRepository mainPlatformTypeRepository, IStoreOuterRefNavigators refNavigators)
        {
            _mainPlatformTypeRepository = mainPlatformTypeRepository;
            _refNavigators = refNavigators;
        }

        public void Create(PlatformType item)
        {
            _mainPlatformTypeRepository.Create(item);
        }

        public void Edit(PlatformType item)
        {
            _mainPlatformTypeRepository.Edit(item);
        }

        public void Delete(PlatformType item)
        {
            _mainPlatformTypeRepository.Delete(item);
        }

        public PlatformType Get(long id)
        {
            return _mainPlatformTypeRepository.Get(id);
        }

        public IEnumerable<PlatformType> Get()
        {
            return _mainPlatformTypeRepository.Get();
        }

        public IEnumerable<PlatformType> Get(Func<PlatformType, bool> predicate)
        {
            return Get().Where(predicate);
        }

        public IEnumerable<PlatformType> GetPlatformTypesForGame(long gameId)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(gameId);
            if (navigator.DatabaseName == UnitOfWork.MainDataBase)
            {
                return _mainPlatformTypeRepository.GetPlatformTypesForGame(navigator.OriginId);
            }
            return Enumerable.Empty<PlatformType>();
        }
    }
}
