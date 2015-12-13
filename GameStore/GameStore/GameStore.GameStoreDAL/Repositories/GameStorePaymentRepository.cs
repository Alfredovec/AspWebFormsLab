using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStorePaymentRepository : IPaymentRepository
    {
        private readonly GameStoreContext _db;

        public GameStorePaymentRepository(GameStoreContext context)
        {
            _db = context;
        }

        public void Create(Payment item)
        {
            _db.Payments.Add(item);
        }

        public void Edit(Payment item)
        {
            _db.SetState(item, EntityState.Modified);
        }

        public void Delete(Payment item)
        {
            _db.Payments.Remove(item);
        }

        public Payment Get(long id)
        {
            return _db.Payments.Find(id);
        }

        public IEnumerable<Payment> Get()
        {
            return _db.Payments.ToList();
        }

        public IEnumerable<Payment> Get(Func<Payment, bool> predicate)
        {
            return _db.Payments.Where(predicate).ToList();
        }
    }
}
