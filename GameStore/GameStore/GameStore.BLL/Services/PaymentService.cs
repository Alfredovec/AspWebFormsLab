using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public PaymentService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.PaymentRepository.Get();
            }
        }

        public Payment GetPayment(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.PaymentRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched payment id: " + id);
                    throw new ArgumentException("Can't find payment", "id");
                }
            }
        }

        public Payment GetPayment(PaymentType type)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.PaymentRepository.Get(p => p.Type == type).First();
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched payment: " + type);
                    throw new ArgumentException("Can't find payment", "type");
                }
            }
        }

        public void CreatePayment(Payment payment)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    _unitOfWork.PaymentRepository.Create(payment);
                    _unitOfWork.Save();
                    _logger.LogInfo(string.Format("Payment created: {0}", payment));
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Key must be unique", "Key");
                }
            }
        }
    }
}
