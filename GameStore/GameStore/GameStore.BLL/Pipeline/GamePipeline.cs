using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class GamePipeline
    {
        private PaginationGameFilter _pagination;

        private IEnumerable<Game> _result;

        public int TotalPages
        {
            get
            {
                return _pagination == null ? 1 : _pagination.TotalPages;
            }
        }

        private IEnumerable<GameBaseFilter> GetFilters(GameFilterModel filterModel)
        {
            if (filterModel.GenreNames != null && filterModel.GenreNames.Count() != 0)
                yield return new GenreGameFilter(filterModel.GenreNames);
            if (filterModel.PlatformTypesNames != null && filterModel.PlatformTypesNames.Count() != 0)
                yield return new PlatformTypeGameFilter(filterModel.PlatformTypesNames);
            if (filterModel.PublishersName != null && filterModel.PublishersName.Count() != 0)
                yield return new PublisherGamePipeline(filterModel.PublishersName);
            if (filterModel.MaxPrice != 0)
                yield return new PriceGameFilter(filterModel.MinPrice, filterModel.MaxPrice);
            if (!string.IsNullOrEmpty(filterModel.Name))
                yield return new NameGameFilter(filterModel.Name);

            yield return new DateGameFilter(filterModel.DateFilter);

            yield return new OrderGameFilter(filterModel.OrderType);

            if (filterModel.PageSize != 0)
            {
                _pagination = new PaginationGameFilter(filterModel.PageSize, filterModel.PageNumber);
                yield return _pagination;
            }

        }

        public IEnumerable<Game> Execute(GameFilterModel filterModel, IEnumerable<Game> games)
        {
            GameBaseFilter root = null;
            GameBaseFilter previous = null;

            foreach (var filter in GetFilters(filterModel))
            {
                if (root == null)
                {
                    root = filter;
                }
                else
                {
                    previous.SetNextFilter(filter);
                }
                previous = filter;
            }
            _result = root == null ? games : root.Execute(games);
            return _result;
        }
    }
}
