using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IPlatformTypeService
    {
        PlatformType GetPlatformType(long id);

        IEnumerable<PlatformType> GetPlatformTypesForGame(long gameId);

        IEnumerable<PlatformType> GetAllPlatformTypes();
    }
}