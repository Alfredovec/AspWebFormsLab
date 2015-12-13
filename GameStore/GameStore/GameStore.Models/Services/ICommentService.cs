using System;
using System.Collections.Generic;
using GameStore.Models.Entities;
using GameStore.Models.Utils;

namespace GameStore.Models.Services
{
    public interface ICommentService
    {
        void CreateComment(Comment comment, string gameKey, List<CommentAction> actions);

        void DeleteComment(long commentId);

        IEnumerable<Comment> GetComments(string gameKey);

        Comment GetComment(long commentId);

        void BanComment(long commentId, TimeSpan period);
    }
}