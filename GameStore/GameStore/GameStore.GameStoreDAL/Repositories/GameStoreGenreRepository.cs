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
    class GameStoreGenreRepository : IGenreRepository
    {
        private readonly GameStoreContext _db;
        private readonly IStoreOuterRefNavigators _refNavigators;

        public GameStoreGenreRepository(GameStoreContext context, IStoreOuterRefNavigators refNavigators)
        {
            _db = context;
            _refNavigators = refNavigators;
        }

        public void Create(Genre item)
        {
            _db.Genres.Add(item);
        }

        public void Edit(Genre item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Genre item)
        {
            item.IsDeleted = true;
            Edit(item);
        }

        public Genre Get(long id)
        {
            var genre = _db.Genres.AsNoTracking().Include(g=>g.Translations).First(g=>g.Id==id);
            return MapGenre(genre);
        }

        private Genre MapGenre(Genre genre)
        {
            genre.Id = _refNavigators.GenreRefNavigator.GenerateGlobalId(genre.Id, DatabaseName.GameStore);
            return genre;
        }

        public IEnumerable<Genre> Get()
        {
            var genres =  _db.Genres.AsNoTracking().Include(g=>g.Translations).Where(g=>!g.IsDeleted).ToList();
            return genres.Select(MapGenre);
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public IEnumerable<Genre> GetGenresByGameId(long gameId)
        {
            return _db.Genres.AsNoTracking().Include(g=>g.Translations).Where(g => !g.IsDeleted).Where(g => g.Games.Any(game => game.Id == gameId))
                .Select(MapGenre).ToList();
        }
    }
}
