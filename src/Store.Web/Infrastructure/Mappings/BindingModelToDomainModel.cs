using System.Collections.Generic;
using Store.Entities;
using Store.Services;
using BM = Store.Web.Models.BM;
using AutoMapper;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.Mappings
{
    public class BindingModelToDomainModel : Profile
    {
        public BindingModelToDomainModel()
            : base("BindingModelToDomainModel")
        {
            // Product
            CreateMap<BM.Product, Product>();
            CreateMap<BM.ProductFilter, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(pf =>
                {
                    IDictionary<string, string> filters = null;

                    if (!string.IsNullOrEmpty(pf.ProductName))
                    {
                        filters = new Dictionary<string, string>();
                        filters["ProductName"] = pf.ProductName;
                    }

                    return filters;
                }));

            // JuridicalPerson
            CreateMap<BM.JuridicalPerson, Customer>();
            CreateMap<BM.JuridicalPerson, JuridicalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.LegalName, opts => opts.MapFrom(src => src.LegalName))
                .ForMember(dest => dest.TIN, opts => opts.MapFrom(src => src.TIN))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<BM.JuridicalPerson, Customer>(src));
                });
            CreateMap<BM.JuridicalPersonFilter, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(jpf =>
                {
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
            CreateMap<BM.NaturalPerson, Customer>();
            CreateMap<BM.NaturalPerson, NaturalPerson>()
                .ForMember(dest => dest.CustomerId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opts => opts.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.Customer, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<BM.NaturalPerson, Customer>(src));
                });
            CreateMap<BM.NaturalPersonFilter, Filtration>()
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
            CreateMap<BM.Order.Details, OrderDetails>();
            CreateMap<BM.Order, Order>()
                .ForMember(dest => dest.OrderDetails, opts =>
                {
                    opts.MapFrom(src => Mapper.Map<List<BM.Order.Details>, List<OrderDetails>>(src.OrderDetails));
                });
            CreateMap<BM.OrderFilter, Filtration>()
                .ForMember(dest => dest.Filters, opts => opts.ResolveUsing(of =>
                {
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

#pragma warning restore 1591
