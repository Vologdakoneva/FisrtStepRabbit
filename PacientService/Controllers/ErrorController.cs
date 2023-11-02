using Microsoft.AspNetCore.Mvc;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IErrorPerson errorPerson;
        private readonly IPublisher publisher;

        public ErrorController(IErrorPerson errorPerson, IPublisher publisher) 
        {
            this.errorPerson = errorPerson;
            this.publisher = publisher;
        }
        // GET: api/<ErrorController>
        [HttpGet]
        public IQueryable<ErrorPerson> Get()
        {
            return errorPerson.GetErrors();
        }

        // GET api/<ErrorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ErrorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ErrorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ErrorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
