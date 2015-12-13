using System;
using System.Collections.Generic;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class PaymentRepository : IPaymentRepository
    {
        private readonly IPaymentRepository _mainPaymentRepository;

        public PaymentRepository(IPaymentRepository mainPaymentRepository)
        {
            _mainPaymentRepository = mainPaymentRepository;
        }

        public void Create(Payment item)
        {
            _mainPaymentRepository.Create(item);
        }

        public void Edit(Payment item)
        {
            _mainPaymentRepository.Edit(item);
        }

        public void Delete(Payment item)
        {
            _mainPaymentRepository.Delete(item);
        }

        public Payment Get(long id)
        {
            return _mainPaymentRepository.Get(id);
        }

        public IEnumerable<Payment> Get()
        {
            return _mainPaymentRepository.Get();
        }

        public IEnumerable<Payment> Get(Func<Payment, bool> predicate)
        {
            return _mainPaymentRepository.Get(predicate);
        }
    }
}
