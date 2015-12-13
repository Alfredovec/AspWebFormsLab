using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        IEnumerable<Genre> GetGenresByGameId(long gameId);
    }
}