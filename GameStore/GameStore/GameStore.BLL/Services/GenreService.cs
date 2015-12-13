using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class GenreService : IGenreService
    {
        private ILogger _logger;

        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Genre GetGenre(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.GenreRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched genreId: " + id);
                    throw new ArgumentException("Can't find genre", "Id");
                }
            }
        }

        public IEnumerable<Genre> GetGenresForGame(long gameId)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GenreRepository.GetGenresByGameId(gameId);
            }
        }


        public IEnumerable<Genre> GetAllGenres()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GenreRepository.Get();
            }
        }
    }
}
