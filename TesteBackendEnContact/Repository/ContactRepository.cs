using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;
        
        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = $"DELETE FROM Contact WHERE Id = {id};";
            await connection.QuerySingleOrDefaultAsync(query);
            await connection.CloseAsync();
        }

        public Task<IEnumerable<Contact>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Contact> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Contact> GetContatosForNome(string nome)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = $"SELECT * FROM Contact WHERE CONTAINS(Name, '{nome}') ";
            var result = await connection.QuerySingleOrDefaultAsync<Contact>(query);
         
            connection.Close();
            return result;
        }

        public async Task<Contact> SaveAsync(Contact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
           
          await connection.InsertAsync(contact);
        
            connection.Close();
            return contact;
        }

        public Task<Company> Update(Company company)
        {
            throw new System.NotImplementedException();
        }

        [Table("Contact")]
        public class ContactDto 
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public int ContactBookId { get; set; }
            public int CompanyId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }

            public ContactDto()
            {
            }

            public ContactDto(Contact contactBook)
            {
                Id = contactBook.Id;
                Name = contactBook.Name;
            }

            public Contact Export() => new Contact(Id, ContactBookId, CompanyId, Name, Phone, Email, Address);
        }
    }
}
