using Store.Data.EntityConfigs;
using Store.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Store", throwIfV1Schema: false)
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        public virtual IDbSet<Customer> Customers { get; set; }

        public virtual IDbSet<JuridicalPerson> JuridicalPersons { get; set; }

        public virtual IDbSet<NaturalPerson> NaturalPersons { get; set; }

        public virtual IDbSet<OrderDetails> OrderDetails { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<Product> Products { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JuridicalPersonConfig());
            modelBuilder.Configurations.Add(new NaturalPersonConfig());
            modelBuilder.Configurations.Add(new CustomerConfig());
            modelBuilder.Configurations.Add(new OrderDetailsConfig());
            modelBuilder.Configurations.Add(new OrderConfig());
            modelBuilder.Configurations.Add(new ProductConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
