using GameStore.Authorization;
using GameStore.Authorization.Interfaces;
using Ninject;
using GameStore.BLL.Services;
using GameStore.DAL.Repositories;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.Configuration
{
    public static class Configuration
    {
        public static void RegisterDependencyInjection(IKernel kernel)
        {
            kernel.Bind<ILogger>().To<Logger.Logger>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IStoreServices>().To<StoreService>();
            kernel.Bind<IAuthorization>().To<GameStoreAuthorization>();
        }
    }
}
