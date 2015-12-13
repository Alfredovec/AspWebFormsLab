using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class PlatformTypeGameFilter : GameBaseFilter
    {
        private readonly IEnumerable<long> _platformsTypes;

        public PlatformTypeGameFilter(IEnumerable<long> platformsTypes)
        {
            _platformsTypes = platformsTypes;
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return list.Where(game =>
                game.PlatformTypes.Any(type =>
                    _platformsTypes.Any(name =>
                        name == type.Id
                        )
                    )
                );
        }
    }
}
