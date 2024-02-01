using ControlPanelRabbit;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using static ControlPanelRabbit.Docsummary;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Loganaliz : ControllerBase
    {
        private readonly IConfiguration configuration;

        public Loganaliz(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public ErrorPerson[] Get()
        {
            string PersonLink = HttpUtility.ParseQueryString(Request.QueryString.Value).Get("PersonLink");
            try
            {
                string urlDocService = configuration.GetConnectionString("PacientService") + "api/Error";
                string Respose = GetAsync(urlDocService+"/"+ PersonLink);
                ErrorPerson[] errorPerson = JsonConvert.DeserializeObject<ErrorPerson[]>(Respose);
                    if (errorPerson == null)
                    errorPerson = new ErrorPerson[0];
                    return errorPerson;
            }
            catch (Exception)
            {
                ErrorPerson[] errorPerson = new ErrorPerson[1];
                errorPerson[0] = new ErrorPerson() { ErrorText = "Ошибка сервиса Log" };
                return errorPerson;

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