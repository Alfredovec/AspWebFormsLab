using System;
using System.Collections.Generic;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class PlatformTypeService : IPlatformTypeService
    {
        private ILogger _logger;

        private readonly IUnitOfWork _unitOfWork;

        public PlatformTypeService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public PlatformType GetPlatformType(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.PlatformTypeRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched platform type: " + id);
                    throw new ArgumentException("Can't find platform type", "Id");
                }
            }
        }

        public IEnumerable<PlatformType> GetPlatformTypesForGame(long gameId)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.PlatformTypeRepository.GetPlatformTypesForGame(gameId);
            }
        }

        public IEnumerable<PlatformType> GetAllPlatformTypes()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.PlatformTypeRepository.Get();
            }
        }
    }
}
