using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain
{
    [Table("Contact")]
    public class Contact
    {
        public Contact(int id, int contactBookId, int companyId, string name, string phone, string email, string address)
        {
            Id = id;
            ContactBookId = contactBookId;
            CompanyId = companyId;
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
        }

        public Contact()
        {
        }

        [Key]
        public int Id { get; set; }
        public int ContactBookId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
