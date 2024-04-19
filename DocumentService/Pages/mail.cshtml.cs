using DocumentService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace DocumentService.shablon
{
    public class mailModel : PageModel
    {
        public UserTasks userTask { get; set; }
        public void OnGet(string UserTaskJSON)
        {
            userTask = JsonConvert.DeserializeObject<UserTasks>(UserTaskJSON);
        }
    }
}
