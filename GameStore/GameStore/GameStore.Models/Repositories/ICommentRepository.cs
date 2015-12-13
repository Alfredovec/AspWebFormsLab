using System.Collections.Generic;
using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetFirstCommentsInThred(long gameId);
    }
}