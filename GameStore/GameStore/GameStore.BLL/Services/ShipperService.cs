using System.Collections.Generic;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ShipperService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IEnumerable<Shipper> GetAll()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.ShipperRepository.Get();
            }
        }

        public Shipper Get(long id)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.ShipperRepository.Get(id);
            }
        }
    }
}
