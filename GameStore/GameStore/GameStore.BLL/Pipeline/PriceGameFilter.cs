using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class PriceGameFilter : GameBaseFilter
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public PriceGameFilter(decimal min, decimal max)
        {
            _minPrice = min;
            _maxPrice = max;
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return list.Where(g => g.Price >= _minPrice && g.Price <= _maxPrice);
        }
    }
}
