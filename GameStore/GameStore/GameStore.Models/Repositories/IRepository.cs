using System;
using System.Collections.Generic;

namespace GameStore.Models.Repositories
{
    public interface IRepository<T>
    {
        void Create(T item);
        
        void Edit(T item);

        void Delete(T item);

        T Get(long id);

        IEnumerable<T> Get();

        IEnumerable<T> Get(Func<T, bool> predicate);

    }
}