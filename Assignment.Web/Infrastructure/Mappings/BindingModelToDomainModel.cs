using Assignment.Entities;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;

namespace Assignment.Web.Infrastructure.Mappings
{
    public class BindingModelToDomainModel : Profile
    {
        public BindingModelToDomainModel()
            : base("BindingModelToDomainModel")
        { }

        protected override void Configure()
        {
            // Product
            CreateMap<ProductBindingModel, Product>();
             
            // JuridicalPerson
            CreateMap<JuridicalPersonBindingModel, Customer>();
            CreateMap<JuridicalPersonBindingModel, JuridicalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.LegalName, opts => opts.MapFrom(src => src.LegalName))
                .ForMember(dest => dest.TIN, opts => opts.MapFrom(src => src.TIN))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<JuridicalPersonBindingModel, Customer>(src));
                });

            // NaturalPerson
            CreateMap<NaturalPersonBindingModel, Customer>();
            CreateMap<NaturalPersonBindingModel, NaturalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opts => opts.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<NaturalPersonBindingModel, Customer>(src));
                });

            // Order
            CreateMap<OrderBindingModel.Details, OrderDetails>();
            CreateMap<OrderBindingModel, Order>()
                .ForMember(dest => dest.OrderDetails, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<IEnumerable<OrderBindingModel.Details>, IEnumerable<OrderDetails>>(src.OrderDetails));
                });
        }
    }
}
