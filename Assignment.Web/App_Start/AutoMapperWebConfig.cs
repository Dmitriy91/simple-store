using Assignment.Web.Infrastructure.Mappings;
using AutoMapper;

namespace Assignment.Web
{
    public static class AutoMapperWebConfig
    {
        public static void RegisterMappingProfiles()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BindingModelToDomainModel>();
                cfg.AddProfile<DomainModelToDto>();
            });
        }
    }
}
