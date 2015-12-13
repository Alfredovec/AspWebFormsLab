using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    public class StoreService : IStoreServices
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public StoreService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ICommentService CommentService
        {
            get { return new CommentService(_unitOfWork, _logger); }
        }

        public IGameService GameService
        {
            get { return new GameService(_unitOfWork, _logger); }
        }

        public IGenreService GenreService
        {
            get { return new GenreService(_unitOfWork, _logger); }
        }

        public IPlatformTypeService PlatformTypeService
        {
            get { return new PlatformTypeService(_unitOfWork, _logger); }
        }

        public IPublisherService PublisherService
        {
            get { return new PublisherService(_unitOfWork, _logger);}
        }

        public IOrderService OrderService
        {
            get { return new OrderService(_unitOfWork, _logger); }
        }

        public IPaymentService PaymentService
        {
            get { return new PaymentService(_unitOfWork, _logger); }
        }

        public IShipperService ShipperService
        {
            get { return new ShipperService(_unitOfWork, _logger); }
        }


        public ITranslateService TranslateService
        {
            get { return new TranslateService(); }
        }

        public IUserService UserService
        {
            get { return new UserService(_unitOfWork, _logger); }
        }
    }
}
