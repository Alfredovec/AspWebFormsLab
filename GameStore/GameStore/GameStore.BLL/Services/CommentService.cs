using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class CommentService : ICommentService
    {
        private readonly ILogger _logger;
        
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public void CreateComment(Comment comment, string gameKey, List<CommentAction> actions)
        {
            using (_logger.LogPerfomance())
            {
                comment.User = comment.User == null ? null : _unitOfWork.UserRepository.GetUser(comment.User.Email);
                var commentRepository = _unitOfWork.CommentRepository;
                Game game;
                try
                {
                    game = _unitOfWork.GameRepository.Get(gameKey);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Can't find game", "gameKey");
                }
                comment.Game = game;

                foreach (var action in actions)
                {
                    switch (action.ActionEnum)
                    {
                        case CommentAction.CommentActionEnum.Reply:
                            ReplyToComment(comment,action.CommentActionId);
                            break;
                        case CommentAction.CommentActionEnum.Quote:
                            QuoteComment(comment, action.CommentActionId);
                            break;
                    }
                }

                commentRepository.Create(comment);
                _logger.LogInfo(string.Format("Comment created: {0}", comment));
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Comment> GetComments(string gameKey)
        {
            using (_logger.LogPerfomance())
            {
                var commentRepository = _unitOfWork.CommentRepository;
                Game game;
                try
                {
                    game = _unitOfWork.GameRepository.Get(gameKey);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Can't find game", "gameKey");
                }
                var comments = commentRepository.GetFirstCommentsInThred(game.Id).ToList();
                return comments;
            }
        }

        public void DeleteComment(long commentId)
        {
            using (_logger.LogPerfomance())
            {
                var comment = _unitOfWork.CommentRepository.Get(commentId);
                if ((comment.Quotes==null||comment.Quotes.Count == 0) &&
                    (comment.Children==null || comment.Children.Count==0))
                {
                    _unitOfWork.CommentRepository.Delete(comment);
                }
                else
                {
                    comment.IsDeleted = true;
                    _unitOfWork.CommentRepository.Edit(comment);
                }
                _logger.LogInfo(string.Format("Comment deleted: {0}", comment));
                _unitOfWork.Save();
            }
        }

        private void ReplyToComment(Comment comment, long parentId)
        {
            comment.Parent = GetComment(parentId);
            if (comment.Parent.IsDeleted)
            {
                _logger.LogInfo("Somebody try hack us");
                throw new InvalidOperationException("You can't reply to deleted comment");
            }
        }

        private void QuoteComment(Comment comment, long quoteId)
        {
            comment.Quote = GetComment(quoteId);
            if (comment.Quote.IsDeleted)
            {
                _logger.LogInfo("Somebody try hack us");
                throw new InvalidOperationException("You can't quote deleted comment");
            }
        }

        public Comment GetComment(long id)
        {
            using (_logger.LogPerfomance())
            {
                try
                {
                    return _unitOfWork.CommentRepository.Get(id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e);
                    throw new ArgumentException("Can't find comment", "id");
                }
            }
        }

        public void BanComment(long commentId, TimeSpan period)
        {
            using (_logger.LogPerfomance())
            {
                var comment = _unitOfWork.CommentRepository.Get(commentId);
                if (comment.User != null)
                {
                    _unitOfWork.UserRepository.BanUser(comment.User.Email, period);
                    _unitOfWork.Save();
                    _logger.LogInfo("Moderator ban user: " + comment.User.Email);
                }
            }
        }
    }
}
