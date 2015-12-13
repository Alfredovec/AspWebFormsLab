using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStoreCommentRepository : ICommentRepository
    {
        private readonly GameStoreContext _db;

        public GameStoreCommentRepository(GameStoreContext context)
        {
            _db = context;
        }

        public void Create(Comment item)
        {
            item.Game = _db.Games.Find(item.Game.Id);
            _db.Comments.Add(item);
        }

        public void Edit(Comment item)
        {
            _db.SetState(item,EntityState.Modified);
        }

        public void Delete(Comment item)
        {
            _db.Comments.Remove(item);
        }

        public Comment Get(long id)
        {
            return _db.Comments.Find(id);
        }

        public IEnumerable<Comment> Get()
        {
            return _db.Comments.ToList();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate)
        {
            return _db.Comments.Where(predicate).ToList();
        }

        public IEnumerable<Comment> GetFirstCommentsInThred(long gameId)
        {
            return _db.Comments.Where(c => c.GameId == gameId && c.Parent == null).ToList();
        }
    }
}
