using Assignment.Web.Infrastructure.Mappings;
using AutoMapper;

#pragma warning disable 1591

namespace Assignment.Web
{
    public static class AutoMapperWebConfig
    {
        public static void RegisterMappingProfiles()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BindingModelToDomainModel>();
                cfg.AddProfile<DomainModelToDTO>();
            });
        }
    }
}

#pragma warning restore 1591
