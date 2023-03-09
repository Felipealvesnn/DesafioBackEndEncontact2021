using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
            connection.Close();
            return dao.Export();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = $"DELETE FROM ContactBook WHERE Id = {id};";
            await connection.QuerySingleOrDefaultAsync(query);
              await connection.CloseAsync();

        }

        public async Task<IEnumerable<IContactBook>> GetAllAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM ContactBook";
            var result = await connection.QueryAsync<ContactBookDao>(query);

            var returnList = new List<IContactBook>();

            foreach (var AgendaSalva in result.ToList())
            {
                IContactBook Agenda = new ContactBook(AgendaSalva.Id, AgendaSalva.Name.ToString());
                returnList.Add(Agenda);
            }
            connection.Close();
            return returnList.ToList();
        }

        public async Task<IContactBook> GetAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = $"SELECT * FROM ContactBook where Id ={id};";
            var result = await connection.QueryFirstOrDefaultAsync<ContactBook>(query);
            connection.Close();
            return result;


        }

        public async Task<IContactBook> GetContatosDaEmpresa(string nome)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = $"SELECT * FROM ContactBook where Id ={nome};";
            var result = await connection.QueryFirstOrDefaultAsync<ContactBook>(query);
            connection.Close();
            return result;


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
                connection.Close();
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
                Id= contactBook.Id;
                Name = contactBook.Name;
            }

            public IContactBook Export() => new ContactBook(Id, Name);
        }
    }
}