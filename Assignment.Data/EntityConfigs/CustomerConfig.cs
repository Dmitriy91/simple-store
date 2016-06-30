using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
{
    public class CustomerConfig : EntityTypeConfiguration<Customer>
    {
        public CustomerConfig()
        {
            HasOptional(c => c.JuridicalPerson)
                .WithRequired(jp => jp.Customer)
                .WillCascadeOnDelete(true);
            HasOptional(c => c.NaturalPerson)
                .WithRequired(np => np.Customer)
                .WillCascadeOnDelete(true);
            HasMany(c => c.Orders)
                .WithRequired()
                .HasForeignKey(o => o.CustomerId);
            Property(c => c.Country)
                .IsRequired()
                .HasMaxLength(64);
            Property(c => c.Region)
                .IsRequired()
                .HasMaxLength(64);
            Property(c => c.City)
                .IsRequired()
                .HasMaxLength(64);
            Property(c => c.StreetAddress)
                .IsRequired()
                .HasMaxLength(64);
            Property(c => c.PostalCode)
                .HasMaxLength(64);
        }
    }
}
