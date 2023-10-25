using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using PromedExchange;
using RabbitMQ.Client;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

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
        // ?PersonLink=d13ab892-6f24-11ec-80e5-000c290cfe98
        // &FamilyPerson=АБАБКО&NamePerson=АРТЕМ&FathersPerson=НИКОЛАЕВИЧ
        // &birthDayPerson=23.04.2006 0:00:00
        // &DataDeath=01.01.0001 0:00:00&Sex_idPerson=1&Sex_Person=Мужской&SocStatus_Person=Учащийся&SocStatus_id_Person=302
        // &viddoc=13&SeriaDoc=19 19&NomDoc=2082010&DataDoc=12.05.2020 0:00:00&KemVidan=УМВД РОССИ ПО ВОЛОГОДСКОЙ ОБЛАСТИ.
        // &SnilsPerson=145-227-158 46&Foms=Согаз-Мед&FomsKod=35003&PolisSeria=&PolisNomer=3595399726002034&IsVrach=False
        [HttpGet]
        public IEnumerable<string> Get(
            [FromQuery(Name = "PersonLink")] string PersonLink,
            [FromQuery(Name = "FamilyPerson")] string FamilyPerson,
            [FromQuery(Name = "NamePerson")] string? NamePerson,
            [FromQuery(Name = "FathersPerson")] string? FathersPerson,
            [FromQuery(Name = "birthDayPerson")] string? birthDayPerson,
            [FromQuery(Name = "Sex_idPerson")] int? Sex_idPerson,
            [FromQuery(Name = "Sex_Person")] string? Sex_Person,
            [FromQuery(Name = "SnilsPerson")] string? SnilsPerson,
            [FromQuery(Name = "PhonePerson")] string? PhonePerson,
            [FromQuery(Name = "Inn_Person")] string? Inn_Person,
            [FromQuery(Name = "SocStatus_Person")] string? SocStatus_Person,
            [FromQuery(Name = "SocStatus_id_Person")] int? SocStatus_id_Person,
            [FromQuery(Name = "viddoc")] int? viddoc,
            [FromQuery(Name = "SeriaDoc")] string? SeriaDoc,
            [FromQuery(Name = "NomDoc")] string? NomDoc,
            [FromQuery(Name = "DataDoc")] string? DataDoc,
            [FromQuery(Name = "KemVidan")] string? KemVidan,
            [FromQuery(Name = "Foms")] string? Foms,
            [FromQuery(Name = "FomsKod")] string? FomsKod,
            [FromQuery(Name = "PolisSeria")] string? PolisSeria,
            [FromQuery(Name = "PolisNomer")] string? PolisNomer,
            [FromQuery(Name = "IsVrach")] bool? IsVrach
            )
        {
            Person person = new Person();
            Guid gPersonLink;
            if (Guid.TryParse(PersonLink, out gPersonLink)) { 
                person.PersonLink = gPersonLink;
                person.FamilyPerson = FamilyPerson;
                person.NamePerson = NamePerson;
                person.FathersPerson = FathersPerson;
                if (birthDayPerson!="")
                person.birthDayPerson = Convert.ToDateTime( birthDayPerson);
                person.SnilsPerson = SnilsPerson;
                person.Sex_idPerson = Sex_idPerson;
                person.Sex_Person = Sex_Person;
                person.SocStatus_Person = SocStatus_Person;
                person.SocStatus_id_Person = SocStatus_id_Person;
                person.Inn_Person = Inn_Person;
                person.viddoc = viddoc;
                person.SeriaDoc = SeriaDoc;
                person.NomDoc = NomDoc;
                if (DataDoc != "")
                    person.DataDoc = Convert.ToDateTime( DataDoc );
                person.KemVidan = KemVidan;
                person.FomsName = Foms;
                person.FomsKod = FomsKod;
                person.PolisSeria = PolisSeria;
                person.PolisNomer = PolisNomer;
                person.IsVrach = IsVrach;


                string personString = JsonConvert.SerializeObject(person);
                Dictionary<string, object> headers = new Dictionary<string, object> { { "", null } };

                publisher.Publish(personString, "pacientkey.update", null);


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
