namespace GameStore.Models.Services
{
    public interface IStoreServices
    {
        ICommentService CommentService { get; }

        IGameService GameService { get; }

        IGenreService GenreService { get; }

        IPlatformTypeService PlatformTypeService { get; }

        IPublisherService PublisherService { get; }

        IOrderService OrderService { get; }

        IPaymentService PaymentService { get; }

        IShipperService ShipperService { get; }

        ITranslateService TranslateService { get; }

        IUserService UserService { get; }
    }
}