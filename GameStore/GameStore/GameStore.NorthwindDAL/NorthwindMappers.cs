using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.NorthwindDAL
{
    class NorthwindMappers
    {
        public void RegisterAllMaps()
        {
            RegisterProductMappers();
            RegisterCategorieMappers();
            RegisterSupplierMappers();
            RegisterOrderMappers();
            RegisterOrderDetailMappers();
            RegisterShipperMappers();
        }

        private void RegisterShipperMappers()
        {
            Mapper.CreateMap<Shippers, Shipper>()
                .ForMember(s => s.Id, m => m.MapFrom(s => s.ShipperID));
        }

        private void RegisterOrderDetailMappers()
        {
            Mapper.CreateMap<Order_Details, OrderDetail>()
                .ForMember(o => o.Discount, m => m.MapFrom(o => o.Discount))
                .ForMember(o => o.ProductId, m => m.MapFrom(o => o.ProductID))
                .ForMember(o => o.Quantity, m => m.MapFrom(o => o.Quantity))
                .ForMember(o=>o.ProductId, m=>m.MapFrom(o=>o.ProductID))
                .ForMember(o => o.Game, m => m.MapFrom(o => o.Products));
        }

        private void RegisterOrderMappers()
        {
            Mapper.CreateMap<Orders, Order>()
                .ForMember(o => o.Id, m => m.MapFrom(o => o.OrderID))
                .ForMember(o => o.CustomerId, m => m.MapFrom(o => o.CustomerID))
                .ForMember(o => o.Date, m => m.MapFrom(o => o.OrderDate))
                .ForMember(o => o.OrderDetails, m => m.MapFrom(o => o.Order_Details))
                .ForMember(o=>o.OrderStatus, m=>m.MapFrom(o=>OrderStatus.Shipped))
                .ForMember(o=>o.ShippedDate, m=>m.MapFrom(o=>o.ShippedDate ?? o.RequiredDate));
        }

        private void RegisterSupplierMappers()
        {
            Mapper.CreateMap<Suppliers, Publisher>()
                .ForMember(p=>p.Id, m=>m.MapFrom(s=>s.SupplierID))
                .ForMember(p => p.CompanyName, m => m.MapFrom(s => s.CompanyName))
                .ForMember(p => p.Translations, m => m.MapFrom(s => new List<PublisherTranslation>{
                    new PublisherTranslation{Description = "Address: " + s.Address, Language = PublisherTranslation.DefaultLanguage}
                }))
                .ForMember(p => p.HomePage, m => m.MapFrom(s => s.HomePage));
        }

        private void RegisterCategorieMappers()
        {
            Mapper.CreateMap<Categories, Genre>()
                .ForMember(c => c.Id, m => m.MapFrom(g => g.CategoryID))
                .ForMember(c => c.Translations, m => m.MapFrom(g => new List<GenreTranslation>()
                {
                    new GenreTranslation { Language = GenreTranslation.DefaultLanguage, Name = g.CategoryName }
                }));
        }

        private void RegisterProductMappers()
        {
            Mapper.CreateMap<Products, Game>()
                .ForMember(g => g.Id, m => m.MapFrom(p => p.ProductID))
                .ForMember(g => g.Name, m => m.MapFrom(p => p.ProductName))
                .ForMember(g => g.Key, m => m.MapFrom(p => "Northwind_" + p.ProductID))
                .ForMember(g => g.Discontinued, m => m.MapFrom(p => p.Discontinued))
                .ForMember(g => g.ContentType, m => m.MapFrom(p => "application/zip"))
                .ForMember(g => g.Price, m => m.MapFrom(p => p.UnitPrice))
                .ForMember(g => g.CreationDate, m => m.MapFrom(p => DateTime.UtcNow.AddYears(-20)))
                .ForMember(g => g.PublishedDate, m => m.MapFrom(p => DateTime.UtcNow.AddYears(-20)))
                .ForMember(g => g.UnitsInStock, m => m.MapFrom(p => p.UnitsInStock))
                .ForMember(g => g.Translations, m => m.MapFrom(p => new List<GameTranslation>(){
                    new GameTranslation { Description = p.ProductName, Language = GameTranslation.DefaultLanguage}
                }))
                .ForMember(g => g.Genres, m => m.MapFrom(p => new List<Categories> {p.Categories}))
                .ForMember(g => g.Publisher, m => m.MapFrom(p => p.Suppliers))
                .ForMember(g => g.PlatformTypes,
                    m => m.MapFrom(p => new List<PlatformType> {new PlatformType {Type = "Northwind"}}));
        }
    }
}
