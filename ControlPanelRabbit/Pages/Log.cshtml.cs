using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.Protocol;


namespace ControlPanelRabbit.Pages
{
    public class Log : PageModel
    {
        private readonly ILogger<PageModel> _logger;

        public Log(ILogger<PageModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                string Respose = GetAsync("http://localhost:39289/api/Error");
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

            //HttpClient _client = new HttpClient();
            //using HttpResponseMessage response = _client.Send( new HttpRequestMessage(HttpMethod.Get, uri));

            return responseString;
        }
    }
}