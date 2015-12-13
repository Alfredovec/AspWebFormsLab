using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class PublisherService : IPublisherService
    {
        private readonly ILogger _logger;
        
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.PublisherRepository.Get();
            }
        }

        public Publisher GetPublisher(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.PublisherRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched company id: " + id);
                    throw new ArgumentException("Can't find publisher", "id");
                }
            }
        }

        public Publisher GetPublisher(string companyName)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.PublisherRepository.Get(p => p.CompanyName == companyName).First();
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched company: " + companyName);
                    throw new ArgumentException("Can't find publisher", "companyName");
                }
            }
        }


        public void CreatePublisher(Publisher publisher)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    _unitOfWork.PublisherRepository.Create(publisher);
                    _unitOfWork.Save();
                    _logger.LogInfo(string.Format("Publisher created: {0}", publisher));
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Key must be unique", "Key");
                }

            }
        }

        public void EditPublisher(Publisher publisher)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.PublisherRepository.Edit(publisher);
                _unitOfWork.Save();
            }
        }

        public void RemovePublisher(long id)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.PublisherRepository.Delete(_unitOfWork.PublisherRepository.Get(id));
            }
        }
    }
}
