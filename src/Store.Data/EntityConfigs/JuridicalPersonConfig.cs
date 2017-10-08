using Store.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Store.Data.EntityConfigs
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
