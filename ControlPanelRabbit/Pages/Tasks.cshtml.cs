using ControlPanelRabbits;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.Protocol;


namespace ControlPanelRabbit.Pages
{
    public class Tasks : PageModel
    {
        private readonly ILogger<PageModel> _logger;
        private readonly IConfiguration configuration;

        public Tasks(ILogger<PageModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }
        public UserTasks[]? UserTask;


        public void OnGet()
        {
            try
            {
                string urlDocService = configuration.GetConnectionString("TaskService");
                string Respose = GetAsync(urlDocService);
                UserTask = JsonConvert.DeserializeObject<UserTasks[]>(Respose);
                
                if (Respose == "") { throw new NotImplementedException(); }
            }
            catch (Exception ex)
            {
                UserTasks[] UserTasks = new UserTasks[1];
                UserTasks[0] = new UserTasks() { Fio = "Ошибка сервиса Документы Задачи" };
                UserTasks = UserTasks;

            }
        }
        public string GetAsync(string uri)
        {
            string responseString = "";
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    // by calling .Result you are synchronously reading the result
                    responseString = responseContent.ReadAsStringAsync().Result;
                    Console.WriteLine(responseString);
                }
            }
            return responseString;
        }
    }
}
