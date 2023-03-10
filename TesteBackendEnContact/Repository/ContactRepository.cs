using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
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
          
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM Contact";
            var result = await connection.QueryAsync<Contact>(query);
           
            return result;
        }

        public async Task<Contact> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Contact>> GetContatosForNome(string String, int pageSize=10, int pageNumber=1)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var offset = (pageNumber - 1) * pageSize;
            var query = $"SELECT * FROM Contact WHERE Name LIKE '%{String}%' OR " +
                        $"Phone LIKE '%{String}%' OR " +
                        $"Email LIKE '%{String}%' OR " +
                        $"Address LIKE '%{String}%' OR " +
                        $"CompanyId IN (SELECT Id FROM Company WHERE Name LIKE '%{String}%') " +
                        $"ORDER BY Name " +
                        $"LIMIT {pageSize} " +
                        $"OFFSET {offset}";
            //var query2 = $"SELECT * FROM Contact WHERE Name LIKE '%{String}%'  ";

           var results = await connection.QueryAsync<Contact>(query);

            return results;
        }

        public async Task<int> SaveAsync(Contact contact)
        {
            
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.InsertAsync(contact);

           
            return result;
        }
        public async Task<int> SaveAsyncList(List<Contact> contact)
        {

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var result = await connection.InsertAsync(contact);


            return result;
        }

        public async Task<Contact> Update(Contact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = $"UPDATE Company SET Name = '{contact.Name}', Address = '{contact.Address}' WHERE Id = {contact.Id}";
            await connection.ExecuteAsync(query);

            return contact;
        }

        [System.ComponentModel.DataAnnotations.Schema.Table("Contact")]
        public class ContactDto
        {
            [System.ComponentModel.DataAnnotations.Key]
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