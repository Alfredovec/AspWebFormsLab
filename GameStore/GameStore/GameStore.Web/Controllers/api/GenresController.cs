using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers.api
{
    public class GenresController : BaseApiController
    {
        public GenresController(IStoreServices storeServices) : base(storeServices) { }

        // GET api/genres
        public IEnumerable<GenreViewModel> Get()
        {
            return _storeServices.GenreService.GetAllGenres().Select(Mapper.Map<GenreViewModel>);
        }

        public HttpResponseMessage Get(long gameId)
        {
            try
            {
                var game = _storeServices.GameService.GetGame(gameId);
                return Request.CreateResponse(HttpStatusCode.OK,
                   game.Genres.Select(Mapper.Map<GenreViewModel>));
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }

        // GET api/genres/5
        public GenreViewModel Get(int id)
        {
            return Mapper.Map<GenreViewModel>(_storeServices.GenreService.GetGenre(id));
        }

        public IEnumerable<GameViewModel> Get(long genreId, FormCollection form)
        {
            return _storeServices.GameService
                .FilterGames(new GameFilterModel { GenreNames = new List<long> { genreId }, PageNumber = 1 })
                .Games.Select(Mapper.Map<GameViewModel>);
        }

        // POST api/genres
        [System.Web.Http.Authorize(Roles = "Manager")]
        public void Post([FromBody]GenreViewModel value)
        {
        }

        // PUT api/genres/5
        [System.Web.Http.Authorize(Roles = "Manager")]
        public void Put(int id, [FromBody]GenreViewModel value)
        {
        }

        // DELETE api/genres/5
        [System.Web.Http.Authorize(Roles = "Manager")]
        public void Delete(int id)
        {
        }
    }
}
