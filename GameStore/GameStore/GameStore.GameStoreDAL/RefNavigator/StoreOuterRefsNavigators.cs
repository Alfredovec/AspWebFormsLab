using GameStore.DALInfrastructure.Interfaces;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL.RefNavigator
{
    class StoreOuterRefsNavigators : IStoreOuterRefNavigators
    {
        private readonly GameStoreContext _db;

        public StoreOuterRefsNavigators(GameStoreContext db)
        {
            _db = db;
        }

        public IRefNavigator<Game> GameRefNavigator
        {
            get { return new GameRefNavigator(_db); }
        }

        public IRefNavigator<Genre> GenreRefNavigator
        {
            get { return new GenreRefNavigator(_db); }
        }

        public IRefNavigator<Publisher> PublisherRefNavigator
        {
            get { return new PublisherRefNavigator(_db); }
        }

        public IRefNavigator<Order> OrderRefNavigator
        {
            get { return new OrderRefNavigator(_db); }
        }
    }
}
