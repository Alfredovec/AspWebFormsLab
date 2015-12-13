using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class CommentRepository : ICommentRepository
    {
        private readonly ICommentRepository _mainCommentRepository;
        private readonly IStoreOuterRefNavigators _refNavigators;
        private readonly UnitOfWork _unitOfWork;

        public CommentRepository(ICommentRepository mainCommentRepository, IStoreOuterRefNavigators refNavigators, UnitOfWork unitOfWork)
        {
            _mainCommentRepository = mainCommentRepository;
            _refNavigators = refNavigators;
            _unitOfWork = unitOfWork;
        }

        public void Create(Comment item)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(item.Game.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                _unitOfWork.GameRepository.Edit(item.Game);
            }
            item.Game.Id = _refNavigators.GameRefNavigator.DecodeGlobalId(item.Game.Id).OriginId;
            _mainCommentRepository.Create(item);
        }

        public void Edit(Comment item)
        {
            _mainCommentRepository.Edit(item);
        }

        public void Delete(Comment item)
        {
            _mainCommentRepository.Delete(item);
        }

        public Comment Get(long id)
        {
            return _mainCommentRepository.Get(id);
        }

        public IEnumerable<Comment> Get()
        {
            return _mainCommentRepository.Get();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate)
        {
            return _mainCommentRepository.Get(predicate);
        }

        public IEnumerable<Comment> GetFirstCommentsInThred(long gameId)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(gameId);
            if (navigator.DatabaseName == UnitOfWork.MainDataBase)
            {
                return _mainCommentRepository.GetFirstCommentsInThred(navigator.OriginId);
            }
            return new List<Comment>();
        }
    }
}
