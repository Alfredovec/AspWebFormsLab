using System;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.Models.Repositories;

namespace GameStore.NorthwindDAL.Repositories
{
    public class NorthwinUnitOfWork : IUnitOfWork
    {
        private readonly Entities _entities;
        private readonly IStoreOuterRefNavigators _refNavigators;

        public NorthwinUnitOfWork(IStoreOuterRefNavigators refNavigators)
        {
            _entities = new Entities();
            var map = new NorthwindMappers();
            map.RegisterAllMaps();
            _refNavigators = refNavigators;
        }

        public ICommentRepository CommentRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IGameRepository GameRepository
        {
            get { return new NorthwinGameRepository(_entities, _refNavigators); }
        }

        public IGenreRepository GenreRepository
        {
            get { return new NorthwinGenreRepository(_entities, _refNavigators); }
        }

        public IPlatformTypeRepository PlatformTypeRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IPublisherRepository PublisherRepository
        {
            get { return new NorthwinPublisherRepository(_entities, _refNavigators); }
        }

        public IPaymentRepository PaymentRepository
        {
            get { throw new NotImplementedException(); }
        }

        public IOrderRepository OrderRepository
        {
            get { return new NorthwinOrderRepository(_entities, _refNavigators); }
        }

        public IShipperRepository ShipperRepository
        {
            get { return new NorthWindShipperRepository(_entities); }
        }

        public IUserRepository UserRepository
        {
            get { throw new NotImplementedException(); }
        }

        public void Save()
        {
        }
    }
}