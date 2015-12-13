using System.Web.Mvc;
using GameStore.Models.Services;

namespace GameStore.Web
{
    public class MapperConfig
    {
        public static void RegisterGlobalMappers()
        {
            IStoreServices services = DependencyResolver.Current.GetService<IStoreServices>();
            var mapper = new GameStoreMappers(services);
            mapper.RegiterAllMaps();
        }
    }
}