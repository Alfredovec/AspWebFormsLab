using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class GameRepository : IGameRepository
    {
        private readonly Dictionary<DatabaseName, IUnitOfWork> _unitOfWorks;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public GameRepository(Dictionary<DatabaseName, IUnitOfWork> unitOfWorks, IStoreOuterRefNavigators refNavigators)
        {
            _unitOfWorks = unitOfWorks;
            _refNavigators = refNavigators;
        }

        public void Create(Game item)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].GameRepository.Create(item);
        }

        public void Edit(Game item)
        {
            var navigator = _refNavigators.GameRefNavigator.GetNavigation(item.Id,TableName.Game);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                Create(item);
                _unitOfWorks[UnitOfWork.MainDataBase].Save();
                _refNavigators.GameRefNavigator.RemoveNavigation(navigator.Id, TableName.Game, item.Id);
            }
            else
            {
                _unitOfWorks[UnitOfWork.MainDataBase].GameRepository.Edit(item);
            }
        }

        public void Delete(long id)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                _refNavigators.GameRefNavigator.RemoveNavigation(id, TableName.Game);
            }
            else
            {
                _unitOfWorks[UnitOfWork.MainDataBase].GameRepository.Delete(navigator.OriginId);
            }
            _unitOfWorks[UnitOfWork.MainDataBase].Save();
        }

        public void Delete(Game item)
        {
            Delete(item.Id);
        }

        public Game Get(long id)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(id);
            try
            {
                var game = _unitOfWorks[navigator.DatabaseName].GameRepository.Get(navigator.OriginId);
                game.Id = id;
                MapGame(game, navigator.DatabaseName);
                return game;
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException();
            }
        }

        private void MapGame(Game game, DatabaseName dbName)
        {
            game.ViewedCount = GetViewCount(game.Id);
            if (game.Comments == null)
            {
                game.Comments = new List<Comment>();
            }
        }

        public IEnumerable<Game> Get()
        {
            var games = new List<Game>();
            foreach (var unitOfWork in _unitOfWorks)
            {
                var localGames = unitOfWork.Value.GameRepository.Get();
                foreach (var game in localGames)
                {
                    MapGame(game, unitOfWork.Key);
                }
                games.AddRange(localGames);
            }
            return games;
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public Game Get(string key)
        {
            foreach (var unitOfWork in _unitOfWorks)
            {
                var game = unitOfWork.Value.GameRepository.Get(key);
                if (game != null)
                {
                    MapGame(game, unitOfWork.Key);
                    return game;
                }
            }
            throw new InvalidOperationException();
        }

        public long CountGames()
        {
            return _unitOfWorks.Sum(unitOfWork => unitOfWork.Value.GameRepository.CountGames());
        }
        
        public void ViewGame(long id)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].GameRepository.ViewGame(id);
        }
        
        public long GetViewCount(long id)
        {
            return _unitOfWorks[UnitOfWork.MainDataBase].GameRepository.GetViewCount(id);
        }
    }
}
