using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactBookRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IContactBook> SaveAsync(IContactBook contactBook)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new ContactBookDao(contactBook);

            dao.Id = await connection.InsertAsync(dao);
            return dao.Export();
        }




        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = $"DELETE FROM ContactBook WHERE Id = {id};";
            await connection.QuerySingleOrDefaultAsync(query);
        }


        public async Task<IEnumerable<IContactBook>> GetAllAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook";
            var result = await connection.QueryAsync<ContactBook>(query);
            return result.ToList();
        }

        public async Task<IContactBook> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = $"SELECT * FROM ContactBook where Id ={id};";
            var result = await connection.QueryFirstOrDefaultAsync<ContactBook>(query);
            return result;
        }

        public async Task<IEnumerable<IContactBook>> GetContatosDaEmpresa(string nome)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = $"SELECT * FROM ContactBook where Name ={nome};";
            var result = await connection.QueryAsync<ContactBook>(query);
            return result.ToList();
        }

        public async Task<IContactBook> Update(IContactBook contactBook)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var testar = await GetAsync(contactBook.Id);
            var dao = new ContactBookDao(testar);
            if (testar != null)
            {
                try
                {
                    await connection.UpdateAsync<ContactBook>((ContactBook)contactBook);
                }
                catch (Exception ex)
                {
                    throw;
                }
              
                return contactBook;
            }

            return contactBook;
        }

        [Table("ContactBook")]
        public class ContactBookDao : IContactBook
        {
            [Key]
            public int Id { get; set; }

            public string Name { get; set; }
            public List<Contact> ListContacts { get; set; }

            public ContactBookDao()
            {
            }

            public ContactBookDao(IContactBook contactBook)
            {
                Id = contactBook.Id;
                Name = contactBook.Name;
            }

            public IContactBook Export() => new ContactBook(Id, Name);
        }
    }
}