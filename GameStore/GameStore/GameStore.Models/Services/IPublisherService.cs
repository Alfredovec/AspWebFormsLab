using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IPublisherService
    {
        IEnumerable<Publisher> GetAllPublishers();

        Publisher GetPublisher(long id);

        Publisher GetPublisher(string companyName);

        void CreatePublisher(Publisher publisher);

        void EditPublisher(Publisher publisher);

        void RemovePublisher(long id);
    }
}
