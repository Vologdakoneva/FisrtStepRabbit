using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PromedExchange
{
    public class Promed
    {
        protected string url = "https://rmisvo.cifromed35.ru";
        protected string? sess_id = "";
        protected bool Logined = false;
        public Promed()
        {
            if (!Logined)
            {
                Login();
            }
        }

        public async Task<string> SendGet(string ApiUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
            using HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
            }
        public async void Login()
        {
            if (sess_id == "")
            {
                string Response = await SendGet("/api/user/login?Login=SelivanovaAM&Password=Clinica_7");
                var jsondoc = JsonDocument.Parse(Response);
                JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
                if (error_code.GetInt32() == 0)
                {
                    sess_id = jsondoc.RootElement.GetProperty("sess_id").GetString();
                    Logined = true;
                }
                else { Logined = false; }
            }
        }
        public async Task<bool> SavePerson(Person person)
        {
            DateTime birthDayPerson = (person.birthDayPerson==null?DateTime.Now.Date: (DateTime)person.birthDayPerson);
            string response = await SendGet("/api/Person" + "?Sess_id="+ sess_id + 
  	                                "&PersonSurName_SurName=" + person.FamilyPerson + 
	                                "&PersonFirName_FirName=" + person.NamePerson +
	                                "&PersonSecName_SecName=" + person.FathersPerson +
	                                "&PersonBirthDay_BirthDay=" + birthDayPerson.ToString("yyyy.MM.dd") +
                                    "&PersonSnils_Snils=" + person.SnilsPerson
                                    );

            return true;
        }
    }
}
