using Assignment.Data.EntityConfigs;
using Assignment.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AssignmentDbConnection1", throwIfV1Schema: false)
        { }

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
            modelBuilder.Configurations.Add(new OrdersDetailsConfig());
            modelBuilder.Configurations.Add(new OrderConfig());
            modelBuilder.Configurations.Add(new ProductConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
