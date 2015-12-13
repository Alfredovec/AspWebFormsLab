using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Pipeline
{
    abstract class BaseFilter<T> : IFilter<T>
    {
        private IFilter<T> nextFilter;

        protected abstract IEnumerable<T> MainExecute(IEnumerable<T> list);

        public void SetNextFilter(IFilter<T> next)
        {
            nextFilter = next;
        }

        public IEnumerable<T> Execute(IEnumerable<T> list)
        {
            var res = MainExecute(list);

            if (nextFilter != null)
            {
                res = nextFilter.Execute(res);
            }
            return res;
        }
    }
}
