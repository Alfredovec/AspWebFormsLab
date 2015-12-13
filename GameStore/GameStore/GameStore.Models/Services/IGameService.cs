using System;
using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IGameService
    {
        void CreateGame(Game game);
        
        void EditGame(Game game);

        void DeleteGame(long id);

        IEnumerable<Game> GetAllGames();

        Game GetGame(long id);

        Game GetGame(string key);

        IEnumerable<Game> GetGameByGenre(long genreId);

        IEnumerable<Game> GetGameByPlatform(long platformTypeId);

        FilterResult FilterGames(GameFilterModel filter);

        long CountGames();

        decimal MaxPrice();

        void ViewGame(string gameKey);

        Tuple<byte[], string> GetImage(long id);

        Tuple<byte[], string> GetImage(string key);
    }
}