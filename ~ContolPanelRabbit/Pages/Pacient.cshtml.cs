using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;

namespace ContolPanelRabbit.Pages
{
    public class Pacient : PageModel
    {
        private readonly ILogger<Pacient> _logger;

        public Pacient(ILogger<Pacient> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            string Respose = await GetAsync("http://localhost:39289/api/Pacient");
            Persons = JsonConvert.DeserializeObject<Person[]>(Respose);
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