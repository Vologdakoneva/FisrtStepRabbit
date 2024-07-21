﻿using Microsoft.AspNetCore.Mvc;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;

//using PromedExchange;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientController : ControllerBase
    {
        private readonly IPerson Person;
        private readonly IPublisher publisher;
        private readonly IConfiguration configuration;
        private readonly PromedExchange.Promed promedexchage;

        public PacientController(IPerson person, IPublisher publisher, IConfiguration configuration)
        {
            this.configuration = configuration;
            promedexchage = new PromedExchange.Promed(false, configuration.GetConnectionString("cifromedLogin"), configuration.GetConnectionString("cifromedPassword"));
            this.Person = person;
            this.publisher = publisher;
        }
        // GET: api/<PacientController>
        [HttpGet( "{Snils},{BirthDay}")]
        public IQueryable<Person> Get(string snils, DateTime BirthDay)
        {
            //Promed pormed = new Promed();
            //pormed.SendGet("");
            return Person.GetPersonsиеBtSnils(snils, BirthDay); //new string[] { "value1", "value2" };
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
        public void Put(Guid id, [FromBody] string value)
        {
        }

        // DELETE api/<PacientController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
