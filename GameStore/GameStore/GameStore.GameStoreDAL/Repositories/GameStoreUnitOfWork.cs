using GameStore.DALInfrastructure.Interfaces;
using GameStore.GameStoreDAL.RefNavigator;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    public class GameStoreUnitOfWork : IUnitOfWork
    {
        private readonly GameStoreContext _db;

        public GameStoreUnitOfWork()
        {
            _db = new GameStoreContext();
        }

        public ICommentRepository CommentRepository
        {
            get { return new GameStoreCommentRepository(_db); }
        }

        public IGameRepository GameRepository
        {
            get { return new GameStoreGameRepository(_db, RefNavigators); }
        }

        public IGenreRepository GenreRepository
        {
            get { return new GameStoreGenreRepository(_db, RefNavigators); }
        }

        public IPlatformTypeRepository PlatformTypeRepository
        {
            get { return new GameStorePlatformTypeRepository(_db); }
        }

        public IPublisherRepository PublisherRepository
        {
            get { return new GameStorePublisherRepository(_db, RefNavigators); }
        }

        public IPaymentRepository PaymentRepository
        {
            get { return new GameStorePaymentRepository(_db); }
        }

        public IShipperRepository ShipperRepository
        {
            get { return null; }
        }

        public IUserRepository UserRepository
        {
            get { return new GameStoreUserRepository(_db); }
        }

        public IOrderRepository OrderRepository
        {
            get { return new GameStoreOrderRepository(_db); }
        }

        public IStoreOuterRefNavigators RefNavigators
        {
            get{return new StoreOuterRefsNavigators(_db);}
        }

        public void Save()
        {
            _db.SaveChanges();
        }        
    }
}