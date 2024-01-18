using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocAnalizController : ControllerBase
    {
        private readonly IDocAnaliz docAnaliz;

        public DocAnalizController(IDocAnaliz docAnaliz)
        {
            this.docAnaliz = docAnaliz;
        }

        // GET: api/<DocAnalizController>
        [HttpGet]
        public IQueryable<DocAnaliz> Get()
        {
            return docAnaliz.GetDocsAnaliz();
        }

        // GET api/<DocAnalizController>/5
        [HttpGet("{id}")]
        public DocAnaliz Get(Guid id)
        {
            return docAnaliz.GetDocAnalizEntity(id);
        }

        // POST api/<DocAnalizController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DocAnalizController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DocAnalizController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
