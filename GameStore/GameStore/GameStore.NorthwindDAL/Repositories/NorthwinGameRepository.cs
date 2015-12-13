using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.NorthwindDAL.Repositories
{
    class NorthwinGameRepository : IGameRepository
    {
        private readonly Entities _entities;

        private readonly IStoreOuterRefNavigators _storeRefs;

        public NorthwinGameRepository(Entities entities, IStoreOuterRefNavigators storeRefs)
        {
            _entities = entities;
            _storeRefs = storeRefs;
        }

        public void Create(Game item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Game item)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Game item)
        {
            throw new NotImplementedException();
        }

        public Game Get(long id)
        {
            var game = Mapper.Map<Game>(_entities.Products.First(p=>p.ProductID==id));
            MapGame(game, _storeRefs.GenreRefNavigator, _storeRefs.PublisherRefNavigator);
            return _storeRefs.GameRefNavigator.SetGlobalRefWithCheckDeletion(game, DatabaseName.Northwind);
        }

        public IEnumerable<Game> Get()
        {
            var games = _entities.Products.Select(Mapper.Map<Game>).ToList();
            var genreRef = _storeRefs.GenreRefNavigator;
            var publisherRef = _storeRefs.PublisherRefNavigator;
            for (int i = 0; i < games.Count; i++)
            {
                MapGame(games[i], genreRef, publisherRef);
            }
            return _storeRefs.GameRefNavigator.SetGlobalRefWithCheckDeletion(games, DatabaseName.Northwind, false);
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public Game Get(string key)
        {
            if (Regex.IsMatch(key, "Northwind_[\\d]+"))
            {
                var keyId = Convert.ToInt64(key.Split('_')[1]);
                var game = Mapper.Map<Game>(_entities.Products.FirstOrDefault(p => p.ProductID == keyId));
                if (game == null)
                {
                    return null;
                }
                MapGame(game, _storeRefs.GenreRefNavigator, _storeRefs.PublisherRefNavigator);
                return _storeRefs.GameRefNavigator.SetGlobalRefWithCheckDeletion(game, DatabaseName.Northwind);
            }
            return null;
        }

        public long CountGames()
        {
            return Get().LongCount();
        }

        private void MapGame(Game game, IRefNavigator<Genre> genreRef, IRefNavigator<Publisher> publisherRef)
        {
            game.Genres = genreRef.SetGlobalRefWithCheckDeletion(game.Genres, DatabaseName.Northwind).ToList();
            game.Publisher = publisherRef.SetGlobalRefWithCheckDeletion(game.Publisher, DatabaseName.Northwind);
        }
        
        public void ViewGame(long id)
        {
            throw new NotImplementedException();
        }

        public long GetViewCount(long id)
        {
            throw new NotImplementedException();
        }
    }
}
