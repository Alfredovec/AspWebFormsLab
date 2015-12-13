using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.BLL.Observer
{
    class PushNotificationObserver : IObserver
    {
        public void Notify(Order order, IEnumerable<User> users)
        {
            return;
        }

        public NotifyStatus Status
        {
            get { return NotifyStatus.PushNotification; }
        }
    }
}