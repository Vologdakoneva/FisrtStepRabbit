using Microsoft.AspNetCore.Mvc;
using MobileApp.Entities;
using Newtonsoft.Json;
using ServiceTask;
using System.ServiceModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        // GET: api/<TakController>
        [HttpGet]
        public IEnumerable<UserTasks> Get()
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            EndpointAddress ea = new EndpointAddress("http://192.168.3.150:8080/ElPol_report_2/WS/Task/1cws/GetAllTask");
            TaskPortTypeClient Service = new TaskPortTypeClient(myBinding, ea);
            Service.ClientCredentials.UserName.UserName = "IIS_USER";
            Service.ClientCredentials.UserName.Password = "457970";
            var ListTask = Service.GetAllTaskAsync().Result;

            string Bodyt = ListTask.Body.@return;
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            UserTasks[] appuser = JsonConvert.DeserializeObject<UserTasks[]>(Bodyt, jsonSettings);
            return appuser.AsEnumerable();
        }

        // GET api/<TakController>/5
        [HttpGet("{taskone}")]
        public string Get(string taskone)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            EndpointAddress ea = new EndpointAddress("http://192.168.3.150:8080/ElPol_report_2/WS/Task/1cws/SaveTask/" + taskone);
            TaskPortTypeClient Service = new TaskPortTypeClient(myBinding, ea);
            Service.ClientCredentials.UserName.UserName = "IIS_USER";
            Service.ClientCredentials.UserName.Password = "457970";
            var ListTask = Service.SaveTaskAsync(taskone).Result;

            
            return "Ok";
        }

        // POST api/<TakController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TakController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TakController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
