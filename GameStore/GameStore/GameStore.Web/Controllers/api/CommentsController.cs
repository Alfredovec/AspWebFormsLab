using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Authorization.Infrastructure;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers.api
{
    public class CommentsController : BaseApiController
    {
        public CommentsController(IStoreServices storeServices) : base(storeServices) { }

        // GET api/comments
        public HttpResponseMessage Get(long gameId)
        {
            try
            {
                var game = _storeServices.GameService.GetGame(gameId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    _storeServices.CommentService.GetComments(game.Key).Select(Mapper.Map<CommentViewModel>));
            }
            catch (ArgumentException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
        }

        // GET api/comments/5
        public HttpResponseMessage Get(long gameId, long id)
        {
            var comment = _storeServices.CommentService.GetComment(id);
            var game = _storeServices.GameService.GetGame(gameId);
            if (comment.Game.Key != game.Key)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                Mapper.Map<CommentViewModel>(comment));
        }

        // POST api/comments
        [System.Web.Http.Authorize(Roles = "Moderator")]
        public HttpResponseMessage Post([FromBody]BanCommentViewModel banComment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var period = new Dictionary<BanCommentViewModel.BanDuration, TimeSpan>();
                    period[BanCommentViewModel.BanDuration.OneHour] = new TimeSpan(0, 1, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneDay] = new TimeSpan(1, 0, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneWeek] = new TimeSpan(7, 0, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneMonth] = new TimeSpan(30, 0, 0, 0);

                    _storeServices.CommentService.BanComment(banComment.CommentId, period[banComment.Ban]);
                    return Request.CreateResponse(HttpStatusCode.Accepted);
                }
                catch (ArgumentException)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment not found");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/comments/5
        public HttpResponseMessage Put(long gameId, [FromBody]CommentViewModel commentViewModel, long parentId=-1, long quoteId=-1)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (User.Identity.IsBanned())
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, GlobalRes.YouAreBanned);
            }
            try
            {
                var actions = new List<CommentAction>();

                if (parentId != -1)
                {
                    actions.Add(new CommentAction
                    {
                        ActionEnum = CommentAction.CommentActionEnum.Reply,
                        CommentActionId = parentId
                    });
                }

                if (quoteId != -1)
                {
                    actions.Add(new CommentAction
                    {
                        ActionEnum = CommentAction.CommentActionEnum.Quote,
                        CommentActionId = quoteId
                    });
                }
                var comment = Mapper.Map<Comment>(commentViewModel);
                comment.Name = User.Identity.GetName();
                comment.User = User.Identity.GetUser();
                var game = _storeServices.GameService.GetGame(gameId);
                _storeServices.CommentService.CreateComment(comment, game.Key, actions);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (InvalidOperationException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            catch (ArgumentException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Game not found");
            }
        }

        [System.Web.Http.Authorize(Roles = "Moderator")]
        // DELETE api/comments/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _storeServices.CommentService.DeleteComment(id);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (ArgumentException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment not found");
            }
        }
    }
}
