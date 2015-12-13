using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class GenreGameFilter : GameBaseFilter
    {
        private readonly IEnumerable<long> _genreIds;

        public GenreGameFilter(IEnumerable<long> genres)
        {
            _genreIds = genres;
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return list.Where(game => 
                game.Genres.Any(genre => 
                    _genreIds.Any(name => 
                        name == genre.Id
                        )
                    )
                );
        }
    }
}
