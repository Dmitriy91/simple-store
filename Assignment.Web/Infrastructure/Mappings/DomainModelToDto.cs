using Assignment.Entities;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;

namespace Assignment.Web.Infrastructure.Mappings
{
    public class DomainModelToDto : Profile
    {
        public DomainModelToDto()
            : base("DomainModelToDto")
        { }

        protected override void Configure()
        {
            // Juridical Person
            CreateMap<JuridicalPerson, JuridicalPersonDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.LegalName, opts => opts.MapFrom(src => src.LegalName))
                .ForMember(dest => dest.TIN, opts => opts.MapFrom(src => src.TIN))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Customer.Country))
                .ForMember(dest => dest.Region, opts => opts.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.StreetAddress, opts => opts.MapFrom(src => src.Customer.StreetAddress))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Customer.PostalCode));

            // Natural Person
            CreateMap<NaturalPerson, NaturalPersonDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Birthdate, opts => opts.MapFrom(src => (src.Birthdate != null) ? src.Birthdate.Value.ToString("yyyy-MM-dd") : string.Empty))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Customer.Country))
                .ForMember(dest => dest.Region, opts => opts.MapFrom(src => src.Customer.Region))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Customer.City))
                .ForMember(dest => dest.StreetAddress, opts => opts.MapFrom(src => src.Customer.StreetAddress))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Customer.PostalCode));

            // Product
            CreateMap<Product, ProductDto>();

            // Order
            CreateMap<OrderDetails, OrderDto.Details>()
                .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => (src.Product == null) ? "Product has been removed." : src.Product.ProductName));
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDate, opts => opts.MapFrom(src => (src.OrderDate.ToString("yyyy-MM-dd"))))
                .ForMember(dest => dest.OrderDetails, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<IEnumerable<OrderDetails>, IEnumerable<OrderDto.Details>>(src.OrderDetails));
                });
        }
    }
}
