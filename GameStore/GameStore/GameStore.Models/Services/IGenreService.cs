using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IGenreService
    {
        Genre GetGenre(long id);
        
        IEnumerable<Genre> GetGenresForGame(long gameId);

        IEnumerable<Genre> GetAllGenres();
    }
}