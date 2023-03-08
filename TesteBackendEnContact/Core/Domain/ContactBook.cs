using System.ComponentModel.DataAnnotations.Schema;
using TesteBackendEnContact.Core.Interface.ContactBook;

namespace TesteBackendEnContact.Core.Domain
{
    [Table("ContactBook")]
    public class ContactBook : IContactBook
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ContactBook(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public ContactBook()
        {
        }
    }
}