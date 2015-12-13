using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.BLL.Pipeline
{
    class OrderGameFilter : GameBaseFilter
    {
        private readonly OrderType _type;
        
        private readonly Dictionary<OrderType, IExpression> _dictionary = new Dictionary<OrderType, IExpression>();

        public OrderGameFilter(OrderType type)
        {
            _type = type;
            _dictionary[OrderType.ByPriceAsc] = Generate(g => -g.Price);
            _dictionary[OrderType.ByPriceDesc] = Generate(g => g.Price); ;
            _dictionary[OrderType.MostCommented] = Generate(g => g.Comments.Count);
            _dictionary[OrderType.MostViewed] = Generate(g => g.ViewedCount);
            _dictionary[OrderType.New] = Generate(g => g.PublishedDate.Ticks);
        }

        protected override IEnumerable<Game> MainExecute(IEnumerable<Game> list)
        {
            return _dictionary[_type].Execute(list);
        }

        private static IExpression Generate<T>(Func<Game, T> func)
        {
            return new MyExpression<T>(func);
        }

        private interface IExpression
        {
            IEnumerable<Game> Execute(IEnumerable<Game> list);
        }

        private class MyExpression<T> : IExpression
        {         
            private readonly Func<Game, T> _func;

            public MyExpression(Func<Game, T> func)
            {
                _func = func;
            }

            public IEnumerable<Game> Execute(IEnumerable<Game> list)
            {
                return list.OrderByDescending(_func);
            }
        }
    }
}
