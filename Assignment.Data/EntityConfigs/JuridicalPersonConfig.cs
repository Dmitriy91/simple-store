using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
{
    public class JuridicalPersonConfig : EntityTypeConfiguration<JuridicalPerson>
    {
        public JuridicalPersonConfig()
        {
            HasKey(jp => jp.CustomerId);
            Property(jp => jp.TIN)
                .IsRequired()
                .HasMaxLength(16);
            Property(jp => jp.LegalName)
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
