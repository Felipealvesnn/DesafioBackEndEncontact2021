using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<int> SaveAsync(Contact company);
        Task<int> SaveAsyncList(List<Contact> company);
        
        Task DeleteAsync(int id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<Contact> GetAsync(int id);

        Task<IEnumerable<Contact>> GetContatosForNome(string String, int pageSize = 10, int pageNumber = 1);

        Task<Contact> Update(Contact company);
    }
}