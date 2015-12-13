using GameStore.Models.Entities;

namespace GameStore.DALInfrastructure.Interfaces
{
    public interface IStoreOuterRefNavigators
    {
        IRefNavigator<Game> GameRefNavigator { get; }

        IRefNavigator<Genre> GenreRefNavigator { get; }

        IRefNavigator<Publisher> PublisherRefNavigator { get; }

        IRefNavigator<Order> OrderRefNavigator { get; } 
    }
}
