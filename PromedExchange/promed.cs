using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PromedExchange
{
    public class Promed
    {
        protected string url = "https://rmisvo.cifromed35.ru";
        protected string sess_id = "";
        protected bool Logined = false;
        public Promed()
        {
            if (!Logined)
            {
                Login();
            }
        }

        public void Login()
        {

        }
        public async void SendGet(string ApiUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
            using HttpResponseMessage response = await httpClient.GetAsync("/api/user/login?Login=SelivanovaAM&Password=Clinica_7");

            response.EnsureSuccessStatusCode();
               

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
    }
}