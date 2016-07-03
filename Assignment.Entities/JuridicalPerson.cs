namespace Assignment.Entities
{
    public class JuridicalPerson
    {
        public int CustomerId { get; set; }

        public string TIN { get; set; } //INN

        public string LegalName { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
