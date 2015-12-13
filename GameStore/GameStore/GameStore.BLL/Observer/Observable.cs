using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;

namespace GameStore.BLL.Observer
{
    class Observable : IObservable
    {
        private readonly List<IObserver> _observers;

        private readonly IUnitOfWork _unitOfWork;

        private Observable(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObserver(Order order)
        {
            var users = _unitOfWork.UserRepository.Get(u => u.Roles.Any(r => r.Name == "Manager")).ToList();
            foreach (var observer in _observers)
            {
                var observer1 = observer;
                observer.Notify(order, users.Where(u => u.ManagerProfile == null ? 
                    NotifyStatus.Mail == observer1.Status : u.ManagerProfile.NotifyStatus == observer1.Status));
            }
        }

        public static Observable CreateObservable(IUnitOfWork unitOfWork)
        {
            var observable = new Observable(unitOfWork);
            observable.AddObserver(new MailObserver());
            observable.AddObserver(new PushNotificationObserver());
            observable.AddObserver(new SmsObserver());
            return observable;
        }
    }
}
