using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileApp.Entities;
using MobileApp.Iterfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceTask;
using System.Net;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text.Json.Serialization;

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsers users;
        private readonly IConfiguration configuration;

        public UserController(IUsers Users, IConfiguration configuration, ServiceTask.TaskPortType taskPort)
        {
            this.users = Users;
            this.configuration = configuration;




        }



        // GET: api/<PacientController>
        [HttpGet]
        public IQueryable<UserApp> Get()
        {
            //Promed pormed = new Promed();
            //pormed.SendGet("");
            return users.GetUsers(); 
        }
        [HttpGet("{id}")]
        public UserApp Get(string id)
        {
            return users.GetUserAppByPhone(id); 
        }
        [HttpGet("{snils},{datar}")]
        public UserApp Get(string snils, string datar)
        {
            return users.GetUserAppByPhone(snils);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            UserApp appuser = JsonConvert.DeserializeObject<UserApp>(value);
            users.SaveUserApp(appuser);
        }
    }
}
