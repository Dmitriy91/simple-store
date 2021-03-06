﻿using Store.Entities;
using DTO = Store.Contracts.Responses;
using AutoMapper;
using System.Collections.Generic;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.Mappings
{
    public class DomainModelToDTO : Profile
    {
        public DomainModelToDTO()
            : base("DomainModelToDTO")
        {
            // Juridical Person
            CreateMap<JuridicalPerson, DTO.JuridicalPerson>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.LegalName, opts => opts.MapFrom(src => src.LegalName))
                .ForMember(dest => dest.TIN, opts => opts.MapFrom(src => src.TIN))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Customer.Country))
                .ForMember(dest => dest.Region, opts => opts.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.StreetAddress, opts => opts.MapFrom(src => src.Customer.StreetAddress))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Customer.PostalCode));

            // Natural Person
            CreateMap<NaturalPerson, DTO.NaturalPerson>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Birthdate, opts => opts.MapFrom(src => src.Birthdate))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Customer.Country))
                .ForMember(dest => dest.Region, opts => opts.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.StreetAddress, opts => opts.MapFrom(src => src.Customer.StreetAddress))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Customer.PostalCode));

            // Product
            CreateMap<Product, DTO.Product>();

            // Order
            CreateMap<OrderDetails, DTO.Order.Details>()
                .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => src.Product == null ? null : src.Product.ProductName));
            CreateMap<Order, DTO.Order>()
                .ForMember(dest => dest.OrderDetails, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<IList<OrderDetails>, List<DTO.Order.Details>>(src.OrderDetails));
                });
        }
    }
}

#pragma warning restore 1591
