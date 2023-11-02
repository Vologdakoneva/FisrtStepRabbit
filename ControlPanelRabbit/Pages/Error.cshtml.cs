using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ControlPanelRabbit.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                string Respose = await GetAsync("http://localhost:39289/api/Pacient");
                errors = JsonConvert.DeserializeObject<ErrorPerson[]>(Respose);
            }
            catch (Exception)
            {
                ErrorPerson[] personserr = new ErrorPerson[1];
                personserr[0] = new ErrorPerson() { ErrorText = "Ошибка сервиса пациенты"};
                errors = personserr;

            }
        }
        public ErrorPerson[]? errors;

        public async Task<string> GetAsync(string uri)
        {
            HttpClient _client = new HttpClient();
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }

    }
}