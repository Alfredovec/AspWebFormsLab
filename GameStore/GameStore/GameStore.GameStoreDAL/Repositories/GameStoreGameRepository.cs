using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL.Model;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStoreGameRepository : IGameRepository
    {
        private readonly GameStoreContext _db;
        private readonly IStoreOuterRefNavigators _refNavigators;

        public GameStoreGameRepository(GameStoreContext context, IStoreOuterRefNavigators refNavigators)
        {
            _db = context;
            _refNavigators = refNavigators;
        }

        public void Create(Game item)
        {
            item.CreationDate = DateTime.UtcNow;
            var genres = item.Genres.ToList();
            var genreNavigator = _refNavigators.GenreRefNavigator;
            foreach (var genre in genres)
            {
                var navigation = genreNavigator.DecodeGlobalId(genre.Id);
                if (navigation.DatabaseName != DatabaseName.GameStore)
                {
                    _db.Genres.Add(genre);
                    _db.SaveChanges();
                    genreNavigator.RemoveNavigation(navigation.Id, TableName.Genre,
                        genreNavigator.GenerateGlobalId(genre.Id, DatabaseName.GameStore));
                }
                else
                {
                    genre.Id = navigation.OriginId;
                    _db.Genres.Attach(genre);
                }
            }
            if (item.Publisher != null)
            {
                var publisherNavigator = _refNavigators.PublisherRefNavigator.DecodeGlobalId(item.Publisher.Id);
                if (publisherNavigator.DatabaseName != DatabaseName.GameStore)
                {
                    _db.Publishers.Add(item.Publisher);
                    _db.SaveChanges();
                    _refNavigators.PublisherRefNavigator.RemoveNavigation(publisherNavigator.Id, TableName.Publisher,
                        _refNavigators.PublisherRefNavigator.GenerateGlobalId(item.Publisher.Id, DatabaseName.GameStore));
                }
                else
                {
                    item.Publisher.Id = publisherNavigator.OriginId;
                    _db.Publishers.Attach(item.Publisher);
                }
            }

            _db.Games.Add(item);
            _db.SaveChanges();
            _db.Entry(item).State = EntityState.Detached;
            item.Id = _refNavigators.GameRefNavigator.GenerateGlobalId(item.Id, DatabaseName.GameStore);
        }

        public void Edit(Game item)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(item.Id);
            var old = _db.Games.Find(navigator.OriginId);
            if (item.ContentType != null)
            {
                old.ContentType = item.ContentType;
            }
            old.Translations.Clear();
            old.Translations = item.Translations;
            old.Key = item.Key;
            old.Price = item.Price;
            old.Name = item.Name;
            old.Publisher = item.Publisher;
            old.PlatformTypes.Clear();
            old.PlatformTypes = item.PlatformTypes;
            old.Genres.Clear();
            old.Genres = item.Genres;
        }

        public void Delete(long id)
        {
            var gameInDb = _db.Games.Find(id);
            gameInDb.IsDeleted = true;
        }

        public void Delete(Game item)
        {
            item.IsDeleted = true;
            Edit(item);
        }

        public Game Get(long id)
        {
            var game = _db.Games.AsNoTracking()
                .Include(g => g.Translations)
                .Include(g => g.PlatformTypes)
                .Include(g => g.Genres.Select(genres => genres.Translations))
                .Include(g => g.Publisher.Translations)
                .First(g => g.Id == id);
            return MapGame(game);
        }

        private Game MapGame(Game game)
        {
            game.Id = _refNavigators.GameRefNavigator.GenerateGlobalId(game.Id, DatabaseName.GameStore);
            if (game.Publisher != null)
            {
                game.Publisher = _refNavigators.PublisherRefNavigator.SetGlobalRefWithCheckDeletion(game.Publisher, DatabaseName.GameStore);
            }
            if (game.Genres.Count != 0)
            {
                game.Genres = _refNavigators.GenreRefNavigator.SetGlobalRefWithCheckDeletion(game.Genres, DatabaseName.GameStore).ToList();
            }
            return game;
        }

        public IEnumerable<Game> Get()
        {
            var games = _db.Games.AsNoTracking()
                .Include(g => g.Translations)
                .Include(g => g.Comments)
                .Include(g => g.PlatformTypes)
                .Include(g => g.Genres.Select(genres => genres.Translations))
                .Include(g => g.Publisher.Translations)
                .Where(g => !g.IsDeleted).ToList();
            return games.Select(MapGame).ToList();
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public Game Get(string key)
        {
            var game = _db.Games.AsNoTracking()
                .Include(g => g.Translations)
                .Include(g => g.Publisher)
                .Include(g => g.PlatformTypes)
                .Include(g => g.Genres.Select(genres => genres.Translations))
                .Include(g => g.Publisher.Translations)
                .FirstOrDefault(g => g.Key == key);
            if (game != null)
            {
                return MapGame(game);
            }
            return null;
        }

        public long CountGames()
        {
            return _db.Games.Where(g => !g.IsDeleted).LongCount();
        }

        public void ViewGame(long id)
        {
            var counter = _db.GameViewCounters.FirstOrDefault(v => v.GlobalGameId == id);
            if (counter == null)
            {
                counter = new GameViewCounter
                {
                    ViewCount = 1,
                    GlobalGameId = id
                };
                _db.GameViewCounters.Add(counter);
            }
            else
            {
                counter.ViewCount++;
                _db.Entry(counter).State = EntityState.Modified;
            }
        }


        public long GetViewCount(long id)
        {
            var counter = _db.GameViewCounters.FirstOrDefault(v => v.GlobalGameId == id);
            return counter == null ? 0 : counter.ViewCount;
        }
    }
}
