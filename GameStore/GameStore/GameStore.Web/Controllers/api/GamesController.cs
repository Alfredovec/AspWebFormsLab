using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers.api
{
    public class GamesController : BaseApiController
    {

        public GamesController(IStoreServices storeServices) : base(storeServices)
        {
        }

        // GET api/games
        public FilterResultViewModel Get([FromUri]GameFilterViewModel filter)
        {
            if (filter == null)
            {
                filter = new GameFilterViewModel();
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
            return result;
        }

        public IEnumerable<GenreViewModel> Get(long gameId)
        {
            var game = _storeServices.GameService.GetGame(gameId);
            return game.Genres.Select(Mapper.Map<GenreViewModel>);
        } 

        // GET api/games/5
        [Authorize]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    Mapper.Map<GameViewModel>(_storeServices.GameService.GetGame(id)));
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }

        // POST api/games
        [Authorize(Roles = "Manager")]
        public HttpResponseMessage Post([FromBody]GameCreateViewModel game)
        {
            if (ModelState.IsValid)
            {
                _storeServices.GameService.CreateGame(Mapper.Map<Game>(game));
                return Request.CreateResponse(HttpStatusCode.Created, "Ok");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/games/5
        [Authorize(Roles = "Manager")]
        public HttpResponseMessage Put(int id, [FromBody]GameCreateViewModel game)
        {
            if (ModelState.IsValid)
            {
                _storeServices.GameService.EditGame(Mapper.Map<Game>(game));
                return Request.CreateResponse(HttpStatusCode.Accepted, "Ok");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE api/games/5
        [Authorize(Roles = "Manager")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _storeServices.GameService.DeleteGame(id);
                return Request.CreateResponse(HttpStatusCode.Accepted, "Ok");
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }
    }
}
