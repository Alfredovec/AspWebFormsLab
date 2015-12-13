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
    [System.Web.Http.Authorize]
    public class OrdersController : BaseApiController
    {
        public OrdersController(IStoreServices storeServices) : base(storeServices) { }

        // GET api/orders/5
        public OrderViewModel Get()
        {
            return Mapper.Map<OrderViewModel>(_storeServices.OrderService.GetOrderByCustomerId(User.Identity.Name));
        }

        // POST api/orders
        public long Post(long gameId, short quantity)
        {
            if (quantity == 0)
            {
                quantity = 1;
            }
            _storeServices.OrderService
                .AddOrderDetail(User.Identity.Name, _storeServices.GameService.GetGame(gameId), quantity);
            return _storeServices.OrderService.GetOrderByCustomerId(User.Identity.Name).Id;
        }

        // PUT api/orders/5
        public HttpResponseMessage Put(long gameId, short quantity)
        {
            if (quantity == 0)
            {
                quantity = 1;
            }
            _storeServices.OrderService
                .AddOrderDetail(User.Identity.Name, _storeServices.GameService.GetGame(gameId), quantity);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // DELETE api/orders/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, id.ToString());
        }
    }
}
