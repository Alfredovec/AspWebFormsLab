using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IShipperService
    {
        IEnumerable<Shipper> GetAll();

        Shipper Get(long id);
    }
}
