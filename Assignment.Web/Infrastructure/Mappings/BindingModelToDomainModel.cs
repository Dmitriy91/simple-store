using System.Collections.Generic;
using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;

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
            CreateMap<ProductBM, Product>();
            CreateMap<ProductFilterBM, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(pf => {
                    IDictionary<string, string> filters = null;

                    if (!string.IsNullOrEmpty(pf.ProductName))
                    {
                        filters = new Dictionary<string, string>();
                        filters["ProductName"] = pf.ProductName;
                    }

                    return filters;
                }));

            // JuridicalPerson
            CreateMap<JuridicalPersonBM, Customer>();
            CreateMap<JuridicalPersonBM, JuridicalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.LegalName, opts => opts.MapFrom(src => src.LegalName))
                .ForMember(dest => dest.TIN, opts => opts.MapFrom(src => src.TIN))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<JuridicalPersonBM, Customer>(src));
                });
            CreateMap<JuridicalPersonFilterBM, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(jpf => {
                    IDictionary<string, string> filters = null;

                    if (!string.IsNullOrWhiteSpace(jpf.LegalName))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["LegalName"] = jpf.LegalName;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.TIN))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["TIN"] = jpf.TIN;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.Country))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.Country"] = jpf.Country;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.Region))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.Region"] = jpf.Region;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.City))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.City"] = jpf.City;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.StreetAddress))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.StreetAddress"] = jpf.StreetAddress;
                    }

                    if (!string.IsNullOrWhiteSpace(jpf.PostalCode))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.PostalCode"] = jpf.PostalCode;
                    }

                    return filters;
                }));

            // NaturalPerson
            CreateMap<NaturalPersonBM, Customer>();
            CreateMap<NaturalPersonBM, NaturalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opts => opts.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<NaturalPersonBM, Customer>(src));
                });
            CreateMap<NaturalPersonFilterBM, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(npf => {
                    IDictionary<string, string> filters = null;

                    if (!string.IsNullOrWhiteSpace(npf.FirstName))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["FirstName"] = npf.FirstName;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.MiddleName))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["MiddleName"] = npf.MiddleName;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.LastName))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["LastName"] = npf.LastName;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.SSN))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["SSN"] = npf.SSN;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.Birthdate))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Birthdate"] = npf.Birthdate;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.Country))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.Country"] = npf.Country;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.Region))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.Region"] = npf.Region;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.City))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.City"] = npf.City;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.StreetAddress))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.StreetAddress"] = npf.StreetAddress;
                    }

                    if (!string.IsNullOrWhiteSpace(npf.PostalCode))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["Customer.PostalCode"] = npf.PostalCode;
                    }

                    return filters;
                }));

            // Order
            CreateMap<OrderBM.Details, OrderDetails>();
            CreateMap<OrderBM, Order>()
                .ForMember(dest => dest.OrderDetails, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<IEnumerable<OrderBM.Details>, IEnumerable<OrderDetails>>(src.OrderDetails));
                });
            CreateMap<OrderFilterBM, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(of => {
                    IDictionary<string, string> filters = null;

                    if (!string.IsNullOrWhiteSpace(of.OrderDate))
                    {
                        if (filters == null)
                            filters = new Dictionary<string, string>();

                        filters["OrderDate"] = of.OrderDate;
                    }

                    return filters;
                }));
        }
    }
}
