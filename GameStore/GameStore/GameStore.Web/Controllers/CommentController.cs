using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Authorization.Infrastructure;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class CommentController : BaseController
    {
        public CommentController(IStoreServices storeServices) : base(storeServices) { }

        public ActionResult Comments(string gameKey, CommentViewModel editingComment)
        {
            try
            {
                ViewBag.EditingCommnet = editingComment;
                var comments = _storeServices.CommentService.GetComments(gameKey)
                    .Select(Mapper.Map<CommentViewModel>).ToList();
                return View(comments);
            }
            catch (ArgumentException)
            {
                return new HttpNotFoundResult("Game not found");
            }
        }

        public ActionResult NewComment(CommentViewModel comment)
        {
            if (string.IsNullOrEmpty(comment.Name) && string.IsNullOrEmpty(comment.Body))
                ModelState.Clear();
            if (TempData.ContainsKey("ModelStateError"))
            {
                ModelState.AddModelError("", (string)TempData["ModelStateError"]);
            }
            return PartialView(comment);
        }

        [HttpPost]
        public ActionResult NewComment(string gameKey, CommentViewModel commentViewModel, long? parentId, long? quoteId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Comments", new { commentViewModel.Name, commentViewModel.Body });
            }
            if (User.Identity.IsBanned())
            {
                TempData["ModelStateError"] = GlobalRes.YouAreBanned;
                return RedirectToAction("Comments", new { commentViewModel.Name, commentViewModel.Body });
            }
            try
            {
                var actions = new List<CommentAction>();

                if (parentId != null)
                {
                    actions.Add(new CommentAction
                    {
                        ActionEnum = CommentAction.CommentActionEnum.Reply,
                        CommentActionId = parentId.Value
                    });
                }

                if (quoteId != null)
                {
                    actions.Add(new CommentAction
                    {
                        ActionEnum = CommentAction.CommentActionEnum.Quote,
                        CommentActionId = quoteId.Value
                    });
                }
                var comment = Mapper.Map<Comment>(commentViewModel);
                comment.Name = User.Identity.GetName();
                comment.User = User.Identity.GetUser();
                _storeServices.CommentService.CreateComment(comment, gameKey, actions);
                return RedirectToAction("Comments");
            }
            catch (InvalidOperationException e)
            {
                TempData["ModelStateError"] = e.Message;
                return RedirectToAction("Comments", new { commentViewModel.Name, commentViewModel.Body });
            }
            catch (ArgumentException)
            {
                return new HttpNotFoundResult("Game not found");
            }
        }

        [HttpGet]
        [LocalizableAuthorize(Roles = "Moderator")]
        public ActionResult Delete(long id, string redirectUrl)
        {
            var comment = _storeServices.CommentService.GetComment(id);
            TempData["redirectUrl"] = redirectUrl;
            TempData.Keep("redirectUrl");
            return View(Mapper.Map<CommentViewModel>(comment));
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Moderator")]
        public ActionResult Delete(long id)
        {
            try
            {
                _storeServices.CommentService.DeleteComment(id);
                return Redirect((string)TempData["redirectUrl"]);
            }
            catch (ArgumentException)
            {
                return new HttpNotFoundResult("Comment not found");
            }
        }

        [HttpGet]
        [LocalizableAuthorize(Roles = "Moderator")]
        public ActionResult Ban(long id, string redirectUrl)
        {
            var comment = _storeServices.CommentService.GetComment(id);
            TempData["redirectUrl"] = redirectUrl;
            TempData.Keep("redirectUrl");
            return View(Mapper.Map<CommentViewModel>(comment));
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Moderator")]
        public ActionResult Ban(BanCommentViewModel banComment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<BanCommentViewModel.BanDuration, TimeSpan> period = new Dictionary<BanCommentViewModel.BanDuration, TimeSpan>();
                    period[BanCommentViewModel.BanDuration.OneHour] = new TimeSpan(0, 1, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneDay] = new TimeSpan(1, 0, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneWeek] = new TimeSpan(7, 0, 0, 0);
                    period[BanCommentViewModel.BanDuration.OneMonth] = new TimeSpan(30, 0, 0, 0);

                    _storeServices.CommentService.BanComment(banComment.CommentId, period[banComment.Ban]);
                    return Redirect((string)TempData["redirectUrl"]);
                }
                catch (ArgumentException)
                {
                    return new HttpNotFoundResult("Comment not found");
                }
            }
            return RedirectToAction("Index", new {controller = "Game"});
        }
    }
}