using System.Collections.Generic;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL.Repositories;
using GameStore.Models.Repositories;
using GameStore.NorthwindDAL.Repositories;

namespace GameStore.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<DatabaseName, IUnitOfWork> _unitOfWorks;

        internal const DatabaseName MainDataBase = DatabaseName.GameStore;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public UnitOfWork()
        {
            _unitOfWorks = new Dictionary<DatabaseName, IUnitOfWork>();
            
            var gameStore = new GameStoreUnitOfWork();
            _refNavigators = gameStore.RefNavigators;

            _unitOfWorks[DatabaseName.GameStore] = gameStore;
            _unitOfWorks[DatabaseName.Northwind] = new NorthwinUnitOfWork(_refNavigators);
        }

        public ICommentRepository CommentRepository
        {
            get { return new CommentRepository(_unitOfWorks[MainDataBase].CommentRepository, _refNavigators, this); }
        }

        public IGameRepository GameRepository
        {
            get { return new GameRepository(_unitOfWorks, _refNavigators); }
        }

        public IGenreRepository GenreRepository
        {
            get { return new GenreRepository(_unitOfWorks, _refNavigators.GenreRefNavigator); }
        }

        public IPlatformTypeRepository PlatformTypeRepository
        {
            get { return new PlatformTypeRepository(_unitOfWorks[MainDataBase].PlatformTypeRepository, _refNavigators); }
        }

        public IPublisherRepository PublisherRepository
        {
            get { return new PublisherRepository(_unitOfWorks, _refNavigators); }
        }

        public IPaymentRepository PaymentRepository
        {
            get { return new PaymentRepository(_unitOfWorks[MainDataBase].PaymentRepository); }
        }

        public IOrderRepository OrderRepository
        {
            get { return new OrderRepository(_unitOfWorks, _refNavigators, this); }
        }

        public IShipperRepository ShipperRepository
        {
            get { return _unitOfWorks[DatabaseName.Northwind].ShipperRepository; }
        }

        public IUserRepository UserRepository
        {
            get { return _unitOfWorks[MainDataBase].UserRepository; }
        }

        public void Save()
        {
            _unitOfWorks[MainDataBase].Save();
        }
    }
}