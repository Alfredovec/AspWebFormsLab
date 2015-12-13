using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.BLL.Interfaces
{
    interface IObserver
    {
        void Notify(Order order, IEnumerable<User> users);

        NotifyStatus Status { get; }
    }
}
