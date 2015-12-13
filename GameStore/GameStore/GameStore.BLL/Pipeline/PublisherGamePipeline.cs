using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class PublisherGamePipeline : GameBaseFilter
    {
        private readonly IEnumerable<long> _publishersName; 

        public PublisherGamePipeline( IEnumerable<long> ids)
        {
            _publishersName = ids;
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return
                list.Where(game => _publishersName.Any(
                            p => game.Publisher != null && !game.Publisher.IsDeleted && p == game.Publisher.Id));
        }
    }
}
