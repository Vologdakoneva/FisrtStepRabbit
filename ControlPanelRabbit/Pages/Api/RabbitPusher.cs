using ContolPanelRabbit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlPanelRabbit.Pages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitPusher : ControllerBase
    {
        private readonly IPublisher publisher;

        public RabbitPusher(IPublisher publisher)
        {
            this.publisher = publisher;
        }
        // GET: api/<RabbitPusher>
        [HttpGet]
        public IEnumerable<string> Get(
            [FromQuery(Name = "PersonLink")] string PersonLink,
            [FromQuery(Name = "FamilyPerson")] string FamilyPerson,
            [FromQuery(Name = "NamePerson")] string NamePerson,
            [FromQuery(Name = "FathersPerson")] string FathersPerson,
            [FromQuery(Name = "birthDayPerson")] DateTime birthDayPerson,
            [FromQuery(Name = "Sex_idPerson")] int Sex_idPerson,
            [FromQuery(Name = "Sex_Person")] string Sex_Person,
            [FromQuery(Name = "SnilsPerson")] string SnilsPerson,
            [FromQuery(Name = "PhonePerson")] string PhonePerson,
            [FromQuery(Name = "SocStatus_Person")] string SocStatus_Person,
            [FromQuery(Name = "Inn_Person")] string Inn_Person
            )
        {
            Person person = new Person();
            Guid gPersonLink;
            if (Guid.TryParse(PersonLink, out gPersonLink)) { 
                person.PersonLink = gPersonLink;
                person.FamilyPerson = FamilyPerson;
                person.NamePerson = NamePerson;
                person.FathersPerson = FathersPerson;
                person.birthDayPerson = birthDayPerson;
                person.Sex_idPerson = Sex_idPerson;
                person.SocStatus_Person = SocStatus_Person;
                person.Inn_Person = Inn_Person;

                string personString = JsonConvert.SerializeObject(person);
            publisher.Publish(personString, "Pacientkey.Update", null);
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/<RabbitPusher>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RabbitPusher>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RabbitPusher>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RabbitPusher>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
