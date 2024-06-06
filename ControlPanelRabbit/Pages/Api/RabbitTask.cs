
using ControlPanelRabbits;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlPanelRabbit.Pages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitTask : ControllerBase
    {
        private readonly IPublisher publisher;

        public RabbitTask(IPublisher publisher)
        {
            this.publisher = publisher;
        }
        // GET: api/<RabbitTask>
        [HttpGet]
        public IEnumerable<string> Get(
            [FromQuery(Name = "PriorityTask")] string PriorityTask,
            [FromQuery(Name = "DataTask")] string DataTask,
            [FromQuery(Name = "FioExec")] string FioExec,
            [FromQuery(Name = "FioFinish")] string? FioFinish,
            [FromQuery(Name = "TextFinish")] string? TextFinish,
            [FromQuery(Name = "DocLink")] string DocLink,
            [FromQuery(Name = "TextTask")] string? TextTask,
            [FromQuery(Name = "ownertask")] string? ownertask,
            [FromQuery(Name = "Fiokey")] string? Fiokey,
            [FromQuery(Name = "IdFio")] Int64? IdFio,
            [FromQuery(Name = "usemail")] bool usemail,
            [FromQuery(Name = "email")] string? email,
            [FromQuery(Name = "telegram")] string? telegram,
            [FromQuery(Name = "usetelegram")] bool usetelegram,
            [FromQuery(Name = "DataTaskPlan")] string DataTaskPlan,
            [FromQuery(Name = "DataFinish")] string? DataFinish



            )
           {
            if (TextTask == null)
                return new string[] { "Оработка", "Сообщение Задачи нет. " };
            

            UserTasks userTask = new UserTasks();

            Guid gDocLink;
            if (Guid.TryParse(DocLink, out gDocLink))
                userTask.DocLink = gDocLink;
            Guid gFiokey;
            if (Guid.TryParse(Fiokey, out gFiokey))
                userTask.Fiokey = Fiokey;

            userTask.FioExec = FioExec;
            userTask.DataTask = DataTask == null ? DateTime.Now.Date : (DateTime)Convert.ToDateTime(DataTask);
            userTask.DataTaskPlan = DataTaskPlan == null ? DateTime.Now.Date : (DateTime)Convert.ToDateTime(DataTaskPlan);
            userTask.DataFinish = DataFinish == null ? DateTime.Now.Date : (DateTime)Convert.ToDateTime(DataFinish);
            userTask.PriorityTask = PriorityTask;
            userTask.FioExec = FioExec;
            userTask.FioFinish = FioFinish;
            userTask.TextTask =  TextTask;
            userTask.ownertask = ownertask;
            userTask.Fiokey = Fiokey;
            userTask.usemail = usemail;
            userTask.email = email;
            userTask.telegram = telegram;
            userTask.usetelegram = usetelegram;

            string personString = JsonConvert.SerializeObject(userTask);
            Dictionary<string, object> headers = new Dictionary<string, object> { { "", null } };

            publisher.Publish(personString, "Task.update", null);

            return new string[] { "Оработка", "Сообщение передано. Анализ" }; 
        }

        // GET api/<RabbitTask>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RabbitTask>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RabbitTask>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RabbitTask>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
