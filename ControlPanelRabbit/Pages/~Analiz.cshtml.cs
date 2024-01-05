using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.Protocol;


namespace ControlPanelRabbit.Pages
{
    public class Analiz : PageModel
    {
        private readonly ILogger<PageModel> _logger;
        private readonly IConfiguration configuration;

        public Analiz(ILogger<PageModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }
        public DocAnaliz[]? docAnaliz;
        public void OnGet()
        {
            try
            {
                string urlDocService = configuration.GetConnectionString("DocumentService");
                string Respose = GetAsync("urlDocService");
                docAnaliz = JsonConvert.DeserializeObject<DocAnaliz[]>(Respose);
                if (Respose == "") { throw new NotImplementedException(); }
            }
            catch (Exception)
            {
                DocAnaliz[] docAnalizs = new DocAnaliz[1];
                docAnalizs[0] = new DocAnaliz() { Fio = "Ошибка сервиса Документы" };
                docAnaliz = docAnalizs;

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
