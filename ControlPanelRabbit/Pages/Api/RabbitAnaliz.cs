
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlPanelRabbit.Pages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitAnaliz : ControllerBase
    {
        private readonly IPublisher publisher;

        public RabbitAnaliz(IPublisher publisher)
        {
            this.publisher = publisher;
        }
        // GET: api/<RabbitAnaliz>
        [HttpGet]
        public IEnumerable<string> Get(
            [FromQuery(Name = "DocLink")] string DocLink,
            [FromQuery(Name = "NomDoc")] string NomDoc,
            [FromQuery(Name = "Datadoc")] string Datadoc,
            [FromQuery(Name = "Fio")] string Fio,
            [FromQuery(Name = "Fiokey")] string Fiokey,
            [FromQuery(Name = "FioDoctor")] string FioDoctor,
            [FromQuery(Name = "FioDoctorkey")] string FioDoctorkey,
            [FromQuery(Name = "Databiomaterial")] string? Databiomaterial,
            [FromQuery(Name = "Items")] string? Items,
            [FromQuery(Name = "AnalizHead")] string? AnalizHead,
            [FromQuery(Name = "UetHead")] string? UetHead
            )
        {
            DocAnaliz docAnaliz = new DocAnaliz();

            Guid gDocLink;
            if (Guid.TryParse(DocLink, out gDocLink))
                docAnaliz.DocLink = gDocLink;
            Guid gFiokey;
            if (Guid.TryParse(Fiokey, out gFiokey))
                docAnaliz.Fiokey = Fiokey;
            Guid gFioDoctorkey;
            if (Guid.TryParse(FioDoctorkey, out gFioDoctorkey))
                docAnaliz.FioDoctorkey = FioDoctorkey;

            docAnaliz.NomDoc = NomDoc;
            docAnaliz.Fio = Fio;
            docAnaliz.FioDoctor = FioDoctor;
            docAnaliz.Databiomaterial = Databiomaterial == null ? DateTime.Now.Date : (DateTime)Convert.ToDateTime(Databiomaterial);
            docAnaliz.Datadoc = Datadoc == null ? DateTime.Now.Date : (DateTime)Convert.ToDateTime(Datadoc);
            docAnaliz.DataChange = DateTime.Now;
            docAnaliz.AnalizHead = AnalizHead;
            docAnaliz.UetHead = UetHead;
            Docsummary docsummary = new Docsummary();

            if (Items != null)
            { 
            List<DocItems> docItems = new List<DocItems>();
            string[] itemAll = Items.Split("|");
            foreach ( string item in itemAll )
            {
                if (item != "") { 
                DocItems docItem = new DocItems();
                string[] StrItem = item.Split("*");
                docItem.DocLink = gDocLink;
                docItem.AnalizText = StrItem[0].Replace('/', '.'); ;
                docItem.norma = StrItem[1].Replace('/', '.'); ;
                docItem.result = StrItem[2].Replace('/','.');
                docItem.uet = StrItem[3];
                docItems.Add(docItem);
                }
            }
            string AnalizString = JsonConvert.SerializeObject(docItems);
            docAnaliz.Items = AnalizString;
            
            docsummary.Items = docItems.ToArray();
            }
            docsummary.docAnaliz = docAnaliz;
            string personString = JsonConvert.SerializeObject(docsummary);
            Dictionary<string, object> headers = new Dictionary<string, object> { { "", null } };

            publisher.Publish(personString, "NaAnaliz.update", null);

            return new string[] { "Оработка", "Сообщение передано. Анализ" }; 
        }

        // GET api/<RabbitAnaliz>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RabbitAnaliz>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RabbitAnaliz>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RabbitAnaliz>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
