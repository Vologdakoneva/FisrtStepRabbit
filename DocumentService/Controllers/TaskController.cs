using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask task;

        public TaskController(ITask task)
        {
            this.task = task;
        }
        // GET: TaskController
        [HttpGet]
        public IQueryable<UserTasks> Get()
        {
            return task.GetTasks(); ;
        }

        
        
    }
}
