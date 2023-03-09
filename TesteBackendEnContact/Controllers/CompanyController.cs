using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TesteBackendEnContact.Controllers.Models;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Core.Interface.ContactBook.Company;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ICompany>> Post(SaveCompanyRequest company, [FromServices] ICompanyRepository companyRepository)
        {
            return Ok(await companyRepository.SaveAsync(company.ToCompany()));
        }

        [HttpDelete]
        public async Task Delete(int id, [FromServices] ICompanyRepository companyRepository)
        {
            await companyRepository.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromServices] ICompanyRepository companyRepository)
        {
            return Ok(await companyRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id, [FromServices] ICompanyRepository companyRepository)
        { var model = await companyRepository.GetAsync(id);

            if (model == null) {

                return NotFound();
            }
            return Ok(model);
           
            
        }
        [HttpPut]
        public async Task<ActionResult> Put(int id, Company company, [FromServices] ICompanyRepository _CompanyRepository)
        {
            if (id != company.Id)
                return BadRequest();

            if (company == null)
                return BadRequest();

            await _CompanyRepository.Update(company);

            return Ok(company);
        }
    }
}