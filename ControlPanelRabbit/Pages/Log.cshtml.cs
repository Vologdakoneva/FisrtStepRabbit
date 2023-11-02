using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ControlPanelRabbit.Pages
{
    public class Log : PageModel
    {
        private readonly ILogger<PageModel> _logger;

        public Log(ILogger<PageModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                string Respose = await GetAsync("http://localhost:39289/api/Error");
                ErrorPerson = JsonConvert.DeserializeObject<ErrorPerson[]>(Respose);
                if (Respose == "") { throw new NotImplementedException(); }
            }
            catch (Exception)
            {
                ErrorPerson[] personserr = new ErrorPerson[1];
                personserr[0] = new ErrorPerson() { ErrorText = "Ошибка сервиса Лог" };
                ErrorPerson = personserr;

            }
        }
        public ErrorPerson[]? ErrorPerson;

        public async Task<string> GetAsync(string uri)
        {
            HttpClient _client = new HttpClient();
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }
    }
}