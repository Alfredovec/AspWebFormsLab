using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class GenreRepository : IGenreRepository
    {
        private readonly Dictionary<DatabaseName, IUnitOfWork> _unitOfWorks;
        private readonly IRefNavigator<Genre> _genreRefNavigator;

        public GenreRepository(Dictionary<DatabaseName, IUnitOfWork> unitOfWorks, IRefNavigator<Genre> genreRefNavigator)
        {            
            _unitOfWorks = unitOfWorks;
            _genreRefNavigator = genreRefNavigator;
        }

        public void Create(Genre item)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].GenreRepository.Create(item);
        }

        public void Edit(Genre item)
        {
            var navigator = _genreRefNavigator.DecodeGlobalId(item.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                Create(item);
                _unitOfWorks[UnitOfWork.MainDataBase].Save();
                _genreRefNavigator.RemoveNavigation(navigator.Id, TableName.Genre, item.Id);
            }
            else
            {
                _unitOfWorks[UnitOfWork.MainDataBase].GenreRepository.Edit(item);
            }
        }

        public void Delete(Genre item)
        {
            var navigator = _genreRefNavigator.DecodeGlobalId(item.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                _genreRefNavigator.RemoveNavigation(item.Id, TableName.Genre);
            }
            else
            {
                item.Id = navigator.OriginId;
                _unitOfWorks[UnitOfWork.MainDataBase].GenreRepository.Delete(item);
            }
        }

        public Genre Get(long id)
        {
            var navigator = _genreRefNavigator.DecodeGlobalId(id);
            var genre = _unitOfWorks[navigator.DatabaseName].GenreRepository.Get(navigator.OriginId);
            return genre;

        }

        public IEnumerable<Genre> Get()
        {
            return _unitOfWorks.SelectMany(u => u.Value.GenreRepository.Get());
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public IEnumerable<Genre> GetGenresByGameId(long gameId)
        {
            var navigator = _genreRefNavigator.DecodeGlobalId(gameId);
            return _unitOfWorks[navigator.DatabaseName].GenreRepository.GetGenresByGameId(navigator.OriginId);
        }
    }
}
