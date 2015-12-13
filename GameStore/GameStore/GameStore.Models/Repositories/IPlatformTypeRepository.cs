using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface IPlatformTypeRepository : IRepository<PlatformType>
    {
        IEnumerable<PlatformType> GetPlatformTypesForGame(long gameId);
    }
}