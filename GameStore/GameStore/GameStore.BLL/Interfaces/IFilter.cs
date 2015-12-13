using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IFilter<T>
    {
        void SetNextFilter(IFilter<T> next);

        IEnumerable<T> Execute(IEnumerable<T> list);
    }
}