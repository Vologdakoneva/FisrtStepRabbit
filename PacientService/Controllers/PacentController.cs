﻿using Microsoft.AspNetCore.Mvc;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;
using PromedExchange;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientController : ControllerBase
    {
        private readonly IPerson Person;
        private readonly IPublisher publisher;

        public PacientController(IPerson person, IPublisher publisher)
        {
            Promed promedexchage;
            promedexchage = new Promed(false);
            this.Person = person;
            this.publisher = publisher;
        }
        // GET: api/<PacientController>
        [HttpGet]
        public IQueryable<Person> Get()
        {
            //Promed pormed = new Promed();
            //pormed.SendGet("");
            return Person.GetPersons(); //new string[] { "value1", "value2" };
        }

        // GET api/<PacientController>/5
        [HttpGet("{id}")]
        public Person Get(Guid id)
        {
            return Person.GetPersonByEntity(id);
        }

        // POST api/<PacientController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PacientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PacientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
