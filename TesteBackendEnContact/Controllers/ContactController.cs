using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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

     

        [HttpGet("Nome")]
        public async Task<ActionResult> GetForNome(string nome, int pageSize = 10, int pageNumber = 1)
        {
            var result = await _IContactRepository.GetContatosForNome(nome, pageSize, pageNumber);
            if (result == null)
            {
                return NotFound(new { Erro = "Nao existe ninguem com esse nome" });
            }
            else
            {
                return Ok(result);
            }
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
            try
            {
                var id = await _IContactRepository.SaveAsync(contato);
                return Ok(new { id = id });
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the contact.");
            }

        }

        // POST api/<ValuesController>
        [HttpPost("Execel")]
        public async Task<ActionResult> PostExcel(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var contacts = new List<Contact>();

                // Ler o arquivo CSV
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Criar um objeto Contact para cada linha do arquivo CSV
                    var contact = new Contact
                    {
                        Id= int.Parse(values[0]),
                        Name = values[1],
                        Email = values[2],
                        Phone = values[3],
                        Address = values[4],
                        CompanyId = int.Parse(values[5]),
                        ContactBookId = int.Parse(values[6]),
                        
                    };

                    contacts.Add(contact);
                }

                // Salvar os contatos no banco de dados ou fazer qualquer outra ação necessária
                // ...
                await _IContactRepository.SaveAsyncList(contacts);
                return Ok();
            }
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