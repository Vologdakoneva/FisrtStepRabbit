using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PromedExchange;

namespace ControlPanelRabbit.Pages
{
    public class Pacient : PageModel
    {
        private readonly ILogger<PageModel> _logger;

        public Pacient(ILogger<PageModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                string Respose = await GetAsync("http://localhost:39289/api/Pacient");
                Persons = JsonConvert.DeserializeObject<Person[]>(Respose);
            }
            catch (Exception)
            {
                Person[] personserr = new Person[1];
                personserr[0] = new Person() { FamilyPerson = "Ошибка сервиса пациенты" , NamePerson="Или сервис недоступен"};
                Persons = personserr;

            }
        }
        public Person[]? Persons;

        public async Task<string> GetAsync(string uri)
        {
            HttpClient _client = new HttpClient();
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }
    }
}