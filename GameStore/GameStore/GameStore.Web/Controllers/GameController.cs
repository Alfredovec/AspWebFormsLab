using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class GameController : BaseController
    {
        private readonly ILogger _logger;


        public GameController(IStoreServices storeServices, ILogger logger) : base(storeServices)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            var filterModel = new GameFilterViewModel
            {
                Genres = _storeServices.GenreService.GetAllGenres().Select(Mapper.Map<GenreViewModel>).ToList(),
                PlatformTypes = _storeServices.PlatformTypeService.GetAllPlatformTypes().Select(Mapper.Map<PlatformTypeViewModel>).ToList(),
                Publishers = _storeServices.PublisherService.GetAllPublishers().Select(Mapper.Map<PublisherViewModel>).ToList(),
                MinPrice = 0,
                MaxPrice = _storeServices.GameService.MaxPrice(),
                PageSize = PageSize.Ten,
                PageNumber = 1
            };
            return View(filterModel);
        }

        public ActionResult FilterGames(GameFilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                if (!ModelState.IsValidField("Name"))
                {
                    filter.Name = "";
                }
            }
            if (filter.PageNumber <= 0)
            {
                filter.PageNumber = 1;
                filter.PageSize = PageSize.Ten;
            }
            var result =
                Mapper.Map<FilterResultViewModel>(_storeServices.GameService.FilterGames(Mapper.Map<GameFilterModel>(filter)));
            result.AwaiblePages = new List<int> { 1, result.PageNumber - 1, result.PageNumber, result.PageNumber + 1, result.TotalPageSize }
                .Distinct().Where(i => i > 0 && i <= result.TotalPageSize).ToList();
            return PartialView(result);
        }

        [HttpGet]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult New()
        {
            var game = new GameCreateViewModel();
            LoadTitles(game);
            return View(game);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult New(GameCreateViewModel game)
        {
            if (!ModelState.IsValid)
            {
                LoadTitles(game);
                return View(game);
            }
            try
            {
                _storeServices.GameService.CreateGame(Mapper.Map<Game>(game));
                return RedirectToAction("Get", new { gameKey = game.Key });
            }
            catch (ArgumentException e)
            {
                LoadTitles(game);
                ModelState.AddModelError(e.ParamName, "Key must be unique");
                return View(game);
            }
        }

        [MonitoringViewed(ParamName = "gameKey")]
        public ActionResult Get(string gameKey)
        {
            try
            {
                var game = _storeServices.GameService.GetGame(gameKey);
                return View(Mapper.Map<GameViewModel>(game));
            }
            catch (ArgumentException)
            {
                return new HttpNotFoundResult("Game not fiund");
            }
        }

        [HttpGet]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Update(string gameKey)
        {
            var game = Mapper.Map<GameCreateViewModel>(_storeServices.GameService.GetGame(gameKey));
            LoadTitles(game);
            return View(game);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Update(GameCreateViewModel game)
        {
            if (!ModelState.IsValid)
            {
                LoadTitles(game);
                return View(game);
            }
            try
            {
                _storeServices.GameService.EditGame(Mapper.Map<Game>(game));
                return RedirectToAction("Get", new { gameKey = game.Key });
            }
            catch (ArgumentException e)
            {
                LoadTitles(game);
                ModelState.AddModelError(e.ParamName, e.Message);
                return View(game);
            }
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Remove(string gameKey)
        {
            var game = Mapper.Map<GameViewModel>(_storeServices.GameService.GetGame(gameKey));
            return View(game);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Remove(long id)
        {
            try
            {
                _storeServices.GameService.DeleteGame(id);
                return RedirectToAction("Manage", "Account");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(e.ParamName, e.Message);
                return new HttpNotFoundResult("Game not found");
            }
        }

        public ActionResult Download(string gameKey)
        {
            Game game;
            try
            {
                game = _storeServices.GameService.GetGame(gameKey);
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(e.ParamName, e.Message);
                return new HttpNotFoundResult("Game not found");
            }
            var fileName = gameKey + "." + game.ContentType.Split('/')[1];
            var path = string.Format("D:\\Games\\Files\\{0}", fileName);
            if (System.IO.File.Exists(path))
                return File(path, game.ContentType, fileName);
            return new HttpNotFoundResult("Game files not found");
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetGameCounts()
        {
            var counts = _storeServices.GameService.CountGames();
            return Content(counts.ToString());
        }

        public FileResult GameImage(long id)
        {
            var imageData = _storeServices.GameService.GetImage(id);
            return File(imageData.Item1, imageData.Item2);
        }

        public async Task<FileResult> GameImageAsync(long id)
        {
            var imageData = await Task.Factory.StartNew(()=>_storeServices.GameService.GetImage(id));
            return File(imageData.Item1, imageData.Item2);
        }

        private void LoadTitles(GameCreateViewModel game)
        {
            game.AllGenres = _storeServices.GenreService.GetAllGenres().Select(Mapper.Map<GenreViewModel>).ToList();
            game.AllPlatforms = _storeServices.PlatformTypeService.GetAllPlatformTypes().Select(Mapper.Map<PlatformTypeViewModel>).ToList();
            game.AllPublishers = _storeServices.PublisherService.GetAllPublishers().Select(Mapper.Map<PublisherViewModel>).ToList();
        }
    }
}
