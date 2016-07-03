using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
{
    public class NaturalPersonConfig : EntityTypeConfiguration<NaturalPerson>
    {
        public NaturalPersonConfig()
        {
            HasKey(np => np.CustomerId);
            Property(np => np.FirstName)
                .IsRequired()
                .HasMaxLength(32);
            Property(np => np.LastName)
                .IsRequired()
                .HasMaxLength(32);
            Property(np => np.MiddleName)
                .IsRequired()
                .HasMaxLength(32);
            Property(np => np.SSN)
                .HasMaxLength(32);
            Property(np => np.Birthdate)
                .HasColumnType("date");
        }
    }
}
