using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.util;
using GameStore.BLL.Pipeline;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class GameService : IGameService
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork _unitOfWork;

        public GameService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public void CreateGame(Game game)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    _unitOfWork.GameRepository.Create(game);
                    _unitOfWork.Save();
                    _logger.LogInfo(string.Format("Game created: {0}", game));
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Key must be unique", "Key");
                }

            }
        }

        public void EditGame(Game game)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    _unitOfWork.GameRepository.Edit(game);
                    _unitOfWork.Save();
                    _logger.LogInfo(string.Format("Game edited: {0}", game));
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Key must be unique", "Key");
                }
            }
        }

        public void DeleteGame(long id)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.GameRepository.Delete(id);
                _unitOfWork.Save();
                _logger.LogInfo(string.Format("Game for id deleted: {0}", id));
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GameRepository.Get();
            }
        }

        public Game GetGame(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.GameRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched gameId: " + id);
                    throw new ArgumentException("Can't find game", "id");
                }
            }
        }

        public Game GetGame(string key)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.GameRepository.Get(key);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Searched game key: " + key);
                    throw new ArgumentException("Can't find game", "key");
                }
            }
        }

        public IEnumerable<Game> GetGameByGenre(long genreId)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GameRepository.Get(g => g.Genres.Any(genre => genre.Id == genreId));
            }
        }

        public IEnumerable<Game> GetGameByPlatform(long platformTypeId)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GameRepository.Get(g => g.PlatformTypes.Any(type => type.Id == platformTypeId));
            }
        }

        public FilterResult FilterGames(GameFilterModel filter)
        {
            using (_logger.LogPerfomance())
            {
                var pipeline = new GamePipeline();
                var games = pipeline.Execute(filter, GetAllGames());
                var result = new FilterResult
                {
                    Games = games,
                    PageNumber = filter.PageNumber,
                    TotalPageSize = pipeline.TotalPages
                };
                return result;
            }
        }

        public long CountGames()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GameRepository.CountGames();
            }
        }


        public decimal MaxPrice()
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.GameRepository.Get().Max(g => g.Price);
            }
        }

        public void ViewGame(string gameKey)
        {
            using (_logger.LogPerfomance())
            {
                var game = GetGame(gameKey);
                _unitOfWork.GameRepository.ViewGame(game.Id);
                _unitOfWork.Save();
            }
        }

        public Tuple<byte[], string> GetImage(long id)
        {
            using (_logger.LogPerfomance())
            {
                var game = _unitOfWork.GameRepository.Get(id);
                if (game == null || game.Picture == null)
                {
                    using (var stream = new MemoryStream())
                    {
                        Properties.Resources._default.Save(stream, ImageFormat.Jpeg);
                        return new Tuple<byte[], string>(stream.ToArray(), "image/jpeg");
                    }
                }
                return new Tuple<byte[], string>(game.Picture, game.ImgMimeType);
            }
        }

        public Tuple<byte[], string> GetImage(string key)
        {
            using (_logger.LogPerfomance())
            {
                Game game = null;
                try
                {
                    game = _unitOfWork.GameRepository.Get(key);
                }
                catch (InvalidOperationException) { }
                if (game == null || game.Picture == null)
                {
                    using (var stream = new MemoryStream())
                    {
                        Properties.Resources._default.Save(stream, ImageFormat.Jpeg);
                        return new Tuple<byte[], string>(stream.ToArray(), "image/jpeg");
                    }
                }
                return new Tuple<byte[], string>(game.Picture, game.ImgMimeType);
            }
        }
    }
}
