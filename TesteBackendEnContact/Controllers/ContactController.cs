using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactRepository _IContactRepository;

        public ContactController(ILogger<ContactController> logger, IContactRepository iContactRepository)
        {
            _logger = logger;
            _IContactRepository = iContactRepository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _IContactRepository.GetAllAsync());
        }

        // GET: api/<ValuesController>
        [HttpGet("Nome")]
        public async Task<ActionResult> GetForNome(string Nome)
        {
            var result = await _IContactRepository.GetContatosForNome(Nome);
            if (result != null) return Ok(result);
            else return Ok(new { Erro = "Nao existe ninguem com esse nome" });
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult> Post(Contact contato)
        {
            var result = await _IContactRepository.SaveAsync(contato);
            if (result == 1) return Ok();
            else return BadRequest();
        }

        // POST api/<ValuesController>
        [HttpPost("Execel")]
        public async Task<ActionResult> PostExcel(Contact contato)
        {
            var result = await _IContactRepository.SaveAsync(contato);
            if (result == 1) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}