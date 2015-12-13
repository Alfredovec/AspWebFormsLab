using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class PublisherRepository : IPublisherRepository
    {
        
        private readonly Dictionary<DatabaseName, IUnitOfWork> _unitOfWorks;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public PublisherRepository(Dictionary<DatabaseName, IUnitOfWork> unitOfWorks, IStoreOuterRefNavigators refNavigators)
        {
            _unitOfWorks = unitOfWorks;
            _refNavigators = refNavigators;
        }

        public void Create(Publisher item)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].PublisherRepository.Create(item);
        }

        public void Edit(Publisher item)
        {
            var navigator = _refNavigators.PublisherRefNavigator.DecodeGlobalId(item.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                Create(item);
                _unitOfWorks[UnitOfWork.MainDataBase].Save();
                _refNavigators.PublisherRefNavigator.RemoveNavigation(navigator.Id, TableName.Publisher,
                    _refNavigators.PublisherRefNavigator.GenerateGlobalId(item.Id, DatabaseName.GameStore));
            }
            else
            {
                _unitOfWorks[UnitOfWork.MainDataBase].PublisherRepository.Edit(item);
            }
        }

        public void Delete(Publisher item)
        {
            var navigator = _refNavigators.PublisherRefNavigator.DecodeGlobalId(item.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                _refNavigators.PublisherRefNavigator.RemoveNavigation(item.Id, TableName.Publisher);
            }
            else
            {
                item.Id = navigator.OriginId;
                _unitOfWorks[UnitOfWork.MainDataBase].PublisherRepository.Delete(item);
            }
        }

        public Publisher Get(long id)
        {
            var navigator = _refNavigators.PublisherRefNavigator.DecodeGlobalId(id);
            var publisher = _unitOfWorks[navigator.DatabaseName].PublisherRepository.Get(navigator.OriginId);
            publisher.Id = id;
            return publisher;
        }

        public IEnumerable<Publisher> Get()
        {
            var t =  _unitOfWorks.SelectMany(u => u.Value.PublisherRepository.Get());
            return t;
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }
    }
}
