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
    public class PublishersController : BaseApiController
    {
        public PublishersController(IStoreServices storeServices) : base(storeServices)
        {
        }

        // GET api/games
        public IEnumerable<PublisherViewModel> Get()
        {
            return _storeServices.PublisherService.GetAllPublishers().Select(Mapper.Map<PublisherViewModel>);
        }

        public IEnumerable<GameViewModel> Get(long publisherId)
        {
            return _storeServices.GameService
                .FilterGames(new GameFilterModel { PublishersName = new List<long> { publisherId }, PageNumber = 1 })
                .Games.Select(Mapper.Map<GameViewModel>);
        }

        // GET api/games/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    Mapper.Map<PublisherViewModel>(_storeServices.PublisherService.GetPublisher(id)));
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }

        // POST api/games
        [System.Web.Http.Authorize(Roles = "Manager")]
        public HttpResponseMessage Post([FromBody]PublisherCreateViewModel publisher)
        {
            if (ModelState.IsValid)
            {
                _storeServices.PublisherService.CreatePublisher(Mapper.Map<Publisher>(publisher));
                return Request.CreateResponse(HttpStatusCode.Created, "Ok");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/games/5
        [System.Web.Http.Authorize(Roles = "Manager")]
        public HttpResponseMessage Put(int id, [FromBody]PublisherCreateViewModel publisher)
        {
            if (ModelState.IsValid)
            {
                _storeServices.PublisherService.EditPublisher(Mapper.Map<Publisher>(publisher));
                return Request.CreateResponse(HttpStatusCode.Accepted, "Ok");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE api/games/5
        [System.Web.Http.Authorize(Roles = "Manager")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _storeServices.PublisherService.RemovePublisher(id);
                return Request.CreateResponse(HttpStatusCode.Accepted, "Ok");
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }
    }
}
