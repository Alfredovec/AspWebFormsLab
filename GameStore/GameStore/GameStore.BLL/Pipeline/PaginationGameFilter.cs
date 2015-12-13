using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    internal class PaginationGameFilter : GameBaseFilter
    {
        private readonly int _pageSize;
        private readonly int _pageNumber;

        public int TotalPages { get; private set; }

        public PaginationGameFilter(int size, int number)
        {
            _pageNumber = number;
            _pageSize = size;
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            TotalPages = (int)Math.Ceiling((decimal) list.Count()/_pageSize);
            return list.Skip(_pageSize*(_pageNumber - 1)).Take(_pageSize);
        }
    }
}
