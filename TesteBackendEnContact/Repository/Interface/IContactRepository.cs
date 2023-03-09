using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<int> SaveAsync(Contact company);

        Task DeleteAsync(int id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<Contact> GetAsync(int id);

        Task<Contact> GetContatosForNome(string nome);

        Task<Company> Update(Company company);
    }
}