using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.BLL.Pipeline
{
    class DateGameFilter : GameBaseFilter
    {
        private readonly DateFilter _type;
        private Dictionary<DateFilter, Func<Game, bool>> dictionary = new Dictionary<DateFilter, Func<Game, bool>>();

        public DateGameFilter(DateFilter type)
        {
            _type = type;
            dictionary[DateFilter.All] = g => true;
            dictionary[DateFilter.LastMonth] = g => g.CreationDate >= DateTime.UtcNow.AddMonths(-1);
            dictionary[DateFilter.LastWeek] = g => g.CreationDate >= DateTime.UtcNow.AddDays(-7);
            dictionary[DateFilter.LastYear] = g => g.CreationDate >= DateTime.UtcNow.AddYears(-1);
            dictionary[DateFilter.TwoYear] = g => g.CreationDate >= DateTime.UtcNow.AddYears(-2);
            dictionary[DateFilter.ThreeYear] = g => g.CreationDate >= DateTime.UtcNow.AddYears(-3);
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return list.Where(dictionary[_type]);
        }
    }
}
