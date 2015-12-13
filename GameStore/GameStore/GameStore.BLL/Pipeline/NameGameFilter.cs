using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;

namespace GameStore.BLL.Pipeline
{
    class NameGameFilter : GameBaseFilter
    {
        private readonly string _name;

        public NameGameFilter(string name)
        {
            _name = name.ToLower();
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return list.Where(g => g.Name.ToLower().Contains(_name));
        }
    }
}
