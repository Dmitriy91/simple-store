using Store.Data;
using Store.Data.Repositories;
using Store.Entities;
using Store.Services;
using Autofac;
using Autofac.Integration.WebApi;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

#pragma warning disable 1591

namespace Store.Web
{
    public class AutofacWebConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ApplicationDbContext>()
                .As<DbContext>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterType<NaturalPersonRepository>()
                .As<IRepository<NaturalPerson>>()
                .InstancePerRequest();

            builder.RegisterType<JuridicalPersonRepository>()
                .As<IRepository<JuridicalPerson>>()
                .InstancePerRequest();

            builder.RegisterType<CustomerRepository>()
                .As<IRepository<Customer>>()
                .InstancePerRequest();

            builder.RegisterType<ProductRepository>()
                .As<IRepository<Product>>()
                .InstancePerRequest();

            builder.RegisterType<OrderRepository>()
                .As<IRepository<Order>>()
                .InstancePerRequest();

            builder.RegisterType<OrderDetailsRepository>()
                .As<IRepository<OrderDetails>>()
                .InstancePerRequest();

            builder.RegisterType<CustomerService>()
                .As<ICustomerService>()
                .InstancePerRequest();

            builder.RegisterType<ProductService>()
                .As<IProductService>()
                .InstancePerRequest();

            builder.RegisterType<OrderService>()
                .As<IOrderService>()
                .InstancePerRequest();

            Container = builder.Build();

            return Container;
        }
    }
}

#pragma warning restore 1591
