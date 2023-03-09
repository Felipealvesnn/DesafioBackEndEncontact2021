using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactBookController : ControllerBase
    {
        private readonly ILogger<ContactBookController> _logger;
        private readonly IContactBookRepository _contactBookRepository;

        public ContactBookController(ILogger<ContactBookController> logger, IContactBookRepository contactBookRepository)
        {
            _logger = logger;
            _contactBookRepository = contactBookRepository;
        }

        [HttpPost]
        public async Task<IContactBook> Post(ContactBook contactBook)
        {
            return await _contactBookRepository.SaveAsync(contactBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = Get(id);
            if (result == null)
            {
                return NotFound();
            }
            await _contactBookRepository.DeleteAsync(id);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _contactBookRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var contactBook = await _contactBookRepository.GetAsync(id);
            if (contactBook == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(contactBook);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, ContactBook contactBook)
        {
            if (id != contactBook.Id)
                return BadRequest();

            if (contactBook == null)
                return BadRequest();

            await _contactBookRepository.Update(contactBook);

            return Ok(contactBook);
        }
    }
}