using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Game Get(string key);

        long CountGames();

        void ViewGame(long id);

        long GetViewCount(long id);

        void Delete(long id);
    }
}