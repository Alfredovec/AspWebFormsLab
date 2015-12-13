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
    class NorthwinGenreRepository : IGenreRepository
    {
        private readonly Entities _entities;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public NorthwinGenreRepository(Entities entities, IStoreOuterRefNavigators refNavigators)
        {
            _entities = entities;
            _refNavigators = refNavigators;
        }

        public void Create(Genre item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Genre item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Genre item)
        {
            throw new NotImplementedException();
        }

        public Genre Get(long id)
        {
            var genre =  Mapper.Map<Genre>(_entities.Categories.Find(id));
            genre = _refNavigators.GenreRefNavigator.SetGlobalRefWithCheckDeletion(genre, DatabaseName.Northwind);
            return genre;
        }

        public IEnumerable<Genre> Get()
        {
            var genres = _entities.Categories.Select(Mapper.Map<Genre>);
            return _refNavigators.GenreRefNavigator.SetGlobalRefWithCheckDeletion(genres, DatabaseName.Northwind, false);
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public IEnumerable<Genre> GetGenresByGameId(long gameId)
        {
            return _entities.Categories.Where(c => c.Products.Any(p => p.ProductID == gameId)).Select(Mapper.Map<Genre>);
        }
    }
}
