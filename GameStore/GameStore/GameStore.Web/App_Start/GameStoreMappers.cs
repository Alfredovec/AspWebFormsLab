using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Services;
using GameStore.Web.Models;

namespace GameStore.Web
{
    public class GameStoreMappers
    {
        private readonly IStoreServices _storeServices;

        private string cultureName
        {
            get { return CultureInfo.CurrentCulture.TwoLetterISOLanguageName; }
        }

        public GameStoreMappers(IStoreServices storeServices)
        {
            _storeServices = storeServices;
        }

        public void RegiterAllMaps()
        {
            RegisterGameMaps();
            RegisterCommentMaps();
            RegisterGenreMaps();
            RegisterPlatformTypeMaps();
            RegisterPublisherMaps();
            RegisterOrderDetailMaps();
            RegisterOrderMaps();
            RegisterPaymentMaps();
            RegisterFilterMaps();
            RegisterOrderTypeMaps();
            RegisterDateFilter();
            RegisterFilterResultMaps();
            RegisterShipperMappers();
            RegisterUserMappers();
            RegisterRoleMappers();
            RegisterCardMappers();
            RegisterManagerProfile();
        }

        private void RegisterManagerProfile()
        {
            Mapper.CreateMap<ManagerProfile, ManagerProfileViewModel>().ReverseMap();
            Mapper.CreateMap<NotifyStatus, NotifyStatusViewModel>().ReverseMap();
        }

        private void RegisterCardMappers()
        {
            Mapper.CreateMap<CardViewModel, CardInfo>();
        }

        private void RegisterRoleMappers()
        {
            Mapper.CreateMap<Role, RoleViewModel>().ReverseMap();
        }

        private void RegisterUserMappers()
        {
            Mapper.CreateMap<RegisterUserViewModel, User>()
                .ForMember(u => u.Roles,
                    m => m.MapFrom(u => new List<Role> { _storeServices.UserService.GetRole("User") }));
            Mapper.CreateMap<UserCreateViewModel, User>()
                .ForMember(u => u.Roles,
                    m => m.MapFrom(u => u.RoleIds.Select(r => _storeServices.UserService.GetRole(r)).ToList()));
            Mapper.CreateMap<User, UserCreateViewModel>()
                .ForMember(u => u.RoleIds, m => m.MapFrom(u => u.Roles.Select(r => r.Id).ToList()));
            Mapper.CreateMap<User, UserDetailsViewModel>()
                .ForMember(u => u.Roles, m => m.MapFrom(u => string.Join(", ", u.Roles.Select(r => r.Name))));
        }

        private void RegisterShipperMappers()
        {
            Mapper.CreateMap<Shipper, ShipperViewModel>();
        }

        private void RegisterFilterResultMaps()
        {
            Mapper.CreateMap<FilterResult, FilterResultViewModel>();
        }

        private void RegisterDateFilter()
        {
            Mapper.CreateMap<DateFilterViewModel, DateFilter>();
        }

        private void RegisterOrderTypeMaps()
        {
            Mapper.CreateMap<OrderTypeViewModel, OrderType>();
        }

        private void RegisterGameMaps()
        {
            Mapper.CreateMap<Game, GameCreateViewModel>()
                .ForMember(gvm => gvm.Genres, m => m.MapFrom(g => g.Genres
                    .Select(genre => genre.Id)))
                .ForMember(gvm => gvm.PlatformTypes, m => m.MapFrom(g => g.PlatformTypes
                    .Select(platform => platform.Id)))
                .ForMember(gvm => gvm.DescriptionRu,
                    m => m.MapFrom(g => _storeServices.TranslateService.GetGameDescription("ru", g.Translations)))
                .ForMember(gvm => gvm.DescriptionEn,
                    m => m.MapFrom(g => _storeServices.TranslateService.GetGameDescription("en", g.Translations)));

            Mapper.CreateMap<Game, GameViewModel>()
                .ForMember(gvm => gvm.Genres, m => m.MapFrom(g => g.Genres.Count == 0 ? new List<string> { "Other" } : g.Genres
                    .Select(genre => _storeServices.TranslateService.GetGenreName(cultureName, genre.Translations))))
                .ForMember(gvm => gvm.PlatformTypes, m => m.MapFrom(g => g.PlatformTypes
                    .Select(platform => platform.Type)))
                .ForMember(gvm => gvm.PublisherName,
                    m => m.MapFrom(g => (g.Publisher == null || g.Publisher.IsDeleted) ? "unknown" : g.Publisher.CompanyName))
                .ForMember(gvm => gvm.Description,
                    m => m.MapFrom(g => _storeServices.TranslateService.GetGameDescription(cultureName, g.Translations)));

            Mapper.CreateMap<GameCreateViewModel, Game>()
                .ForMember(g => g.Genres, m => m.MapFrom(gvm => gvm.Genres == null ? null : gvm.Genres
                    .Select(id => _storeServices.GenreService.GetGenre(id))))
                .ForMember(g => g.PlatformTypes, m => m.MapFrom(gvm => gvm.PlatformTypes
                    .Select(id => _storeServices.PlatformTypeService.GetPlatformType(id))))
                .ForMember(g => g.Publisher, m => m.MapFrom(gvm =>
                    gvm.PublisherId == null ? null : _storeServices.PublisherService.GetPublisher(gvm.PublisherId.Value)))
                .ForMember(g => g.Translations, m => m.MapFrom(gvm =>
                    new List<GameTranslation>
                    {
                        new GameTranslation { Language = Language.Ru, Description = gvm.DescriptionRu}, 
                        new GameTranslation { Language = Language.En, Description = gvm.DescriptionEn}
                    }
                    ));
        }

        private void RegisterCommentMaps()
        {
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(c => c.QuoteName, m => m.MapFrom(c => c.Quote.Name))
                .ForMember(c => c.QuoteText, m => m.MapFrom(c => c.Quote.Body));
            Mapper.CreateMap<CommentViewModel, Comment>();
        }

        private void RegisterGenreMaps()
        {
            Mapper.CreateMap<Genre, GenreViewModel>()
                .ForMember(gvm => gvm.Name,
                    m => m.MapFrom(g => _storeServices.TranslateService.GetGenreName(cultureName, g.Translations)));
            Mapper.CreateMap<GenreViewModel, Genre>();
        }

        private void RegisterPlatformTypeMaps()
        {
            Mapper.CreateMap<PlatformType, PlatformTypeViewModel>();
            Mapper.CreateMap<PlatformTypeViewModel, PlatformType>();
        }

        private void RegisterPublisherMaps()
        {
            Mapper.CreateMap<Publisher, PublisherViewModel>()
                .ForMember(p => p.Description, m => m.MapFrom(p => _storeServices.TranslateService.GetPublisherDescrition(cultureName, p.Translations)));
            Mapper.CreateMap<PublisherViewModel, Publisher>();
            Mapper.CreateMap<PublisherCreateViewModel, Publisher>()
                .ForMember(g => g.Translations, m => m.MapFrom(gvm =>
                    new List<PublisherTranslation>
                    {
                        new PublisherTranslation {Language = Language.Ru, Description = gvm.DescriptionRu},
                        new PublisherTranslation {Language = Language.En, Description = gvm.DescriptionEn}
                    }
                    ));
            Mapper.CreateMap<Publisher, PublisherCreateViewModel>()
                .ForMember(p => p.DescriptionRu,
                    m => m.MapFrom(
                            p => _storeServices.TranslateService.GetPublisherDescrition("ru", p.Translations)))
                .ForMember(p => p.DescriptionEn,
                    m => m.MapFrom(
                            p => _storeServices.TranslateService.GetPublisherDescrition("en", p.Translations)));
        }

        private void RegisterOrderDetailMaps()
        {
            Mapper.CreateMap<OrderDetail, OrderDetailViewModel>();
            Mapper.CreateMap<OrderDetailViewModel, OrderDetail>();
        }

        private void RegisterOrderMaps()
        {
            Mapper.CreateMap<Order, OrderViewModel>();
            Mapper.CreateMap<OrderViewModel, Order>();
        }

        private void RegisterPaymentMaps()
        {
            Mapper.CreateMap<Payment, PaymentViewModel>();
        }

        private void RegisterFilterMaps()
        {
            Mapper.CreateMap<GameFilterViewModel, GameFilterModel>()
                .ForMember(g => g.PageSize, m => m.MapFrom(g => (int)g.PageSize));
        }
    }
}