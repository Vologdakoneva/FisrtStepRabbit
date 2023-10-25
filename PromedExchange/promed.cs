using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PromedExchange
{
    public class Promed
    {
        protected string url = "https://rmisvo.cifromed35.ru";
        protected string? sess_id = "";
        protected bool Logined = false;
        public string response = "";
        public string error_msg = "";
        JsonDocument? jsondoc = null;
        HttpWebResponse? Resp;
        public Promed(bool needAsync)
        {
            if (!Logined)
            {
                if (needAsync) {
                    // LoginAsync();
                }
                else
                    Login();
            }
        }

        public string SendPost(string ApiUrl, string Data)
        {
            CookieContainer SessionCookieHolder = new CookieContainer();
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url + ApiUrl);
                if (sess_id != "")
                    WebReq.Headers.Add("Cookie", "PHPSESSID=" + sess_id);
                WebReq.Method = "POST"; //GET/POST/HEAD depending on the request type//
                WebReq.KeepAlive = true;
                var bytes = Encoding.ASCII.GetBytes(Data);
                using (var requestStream = WebReq.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                Resp = (HttpWebResponse)WebReq.GetResponse();
                var encoding = ASCIIEncoding.UTF8;
                using (var reader = new System.IO.StreamReader(Resp.GetResponseStream(), encoding))
                {
                    response = reader.ReadToEnd();
                }

            }

            catch (Exception ex)

            {
                string ExceptionReader = ex.Message;
            }
            if (Resp.StatusCode != HttpStatusCode.OK) response = "";
            
            return response;  //jsonResponse;
        }
        public bool SendPut(string ApiUrl, string Data)
        {
            CookieContainer SessionCookieHolder = new CookieContainer();
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url + ApiUrl);
                if (sess_id != "")
                    WebReq.Headers.Add("Cookie", "PHPSESSID=" + sess_id);
                WebReq.Method = "PUT"; //GET/POST/HEAD depending on the request type//
                WebReq.KeepAlive = true;
                var bytes = Encoding.ASCII.GetBytes(Data);
                using (var requestStream = WebReq.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                Resp = (HttpWebResponse)WebReq.GetResponse();
            }

            catch (Exception ex)

            {
                string ExceptionReader = ex.Message;
            }
            return Resp.StatusCode == HttpStatusCode.OK;  //jsonResponse;
        }
        public string SendGet(string ApiUrl)
        {
            CookieContainer SessionCookieHolder = new CookieContainer(); 
            if (sess_id != "") { 
            Cookie SessionCookie = new Cookie();
            SessionCookie.Name = "Cookie";
            SessionCookie.Value = "PHPSESSID=" + sess_id;
                SessionCookie.Domain = "cifromed35.ru";

            SessionCookieHolder.Add(SessionCookie);
            }
            try
            {
                
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url+ ApiUrl);
                if (sess_id != "")
                    WebReq.Headers.Add("Cookie", "PHPSESSID=" + sess_id);
                //WebReq.CookieContainer = SessionCookieHolder;
                WebReq.Method = "GET"; //GET/POST/HEAD depending on the request type//
                WebReq.KeepAlive = true;
                HttpWebResponse Resp = (HttpWebResponse)WebReq.GetResponse();
                var encoding = ASCIIEncoding.UTF8;
                using (var reader = new System.IO.StreamReader(Resp.GetResponseStream(), encoding))
                {
                    response = reader.ReadToEnd();
                }
            }

            catch (Exception ex)

            {
                string ExceptionReader = ex.Message;
            }
            return response;  //jsonResponse;
        }
        public async Task<string> SendGetAsync(string ApiUrl)
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

        public bool Login()
        {
            if (sess_id == "")
            {

                response = SendGet("/api/user/login?Login=SelivanovaAM&Password=Clinica_7");
                var jsondoc = JsonDocument.Parse(response);
                JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
                if (error_code.GetInt32() == 0)
                {
                    sess_id = jsondoc.RootElement.GetProperty("sess_id").GetString();
                    Logined = true;
                }
                else { Logined = false; }
            }
            return Logined;
        }

        public async Task<bool> LoginAsync()
        {
            if (sess_id == "")
            {
                response = await SendGetAsync("api/user/login?Login=SelivanovaAM&Password=Clinica_7");
                var jsondoc = JsonDocument.Parse(response);
                JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
                if (error_code.GetInt32() == 0)
                {
                    sess_id = jsondoc.RootElement.GetProperty("sess_id").GetString();
                    Logined = true;
                }
                else { Logined = false; }
            }
            return await Task.FromResult(Logined);
        }
        public async Task<bool> GetPersonAsync(Person person)
        {
            int idperson = -1;
            DateTime birthDayPerson = (person.birthDayPerson == null ? DateTime.Now.Date : (DateTime)person.birthDayPerson);
            response = await SendGetAsync("/api/Person" + "?Sess_id=" + sess_id +
                                      "&PersonSurName_SurName=" + person.FamilyPerson +
                                    "&PersonFirName_FirName=" + person.NamePerson +
                                    "&PersonSecName_SecName=" + person.FathersPerson +
                                    "&PersonBirthDay_BirthDay=" + birthDayPerson.ToString("yyyy-MM-dd") +
                                    "&PersonSnils_Snils=" + person.SnilsPerson
                                    );
            var jsondoc = JsonDocument.Parse(response);
            JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
            if (error_code.GetInt32() == 0)
            {
                JsonElement data = jsondoc.RootElement.GetProperty("data");
                idperson = data[0].GetProperty("Person_id").GetInt32();
            }

            return await Task.FromResult(true);
        }
        public int GetPerson(Person person)
        {
            int idperson = -1;
            DateTime birthDayPerson = (person.birthDayPerson == null ? DateTime.Now.Date : (DateTime)person.birthDayPerson);
            response = SendGet("/api/Person" + "?Sess_id=" + sess_id +
                                      "&PersonSurName_SurName=" + person.FamilyPerson +
                                    "&PersonFirName_FirName=" + person.NamePerson +
                                    "&PersonSecName_SecName=" + person.FathersPerson +
                                    "&PersonBirthDay_BirthDay=" + birthDayPerson.ToString("yyyy-MM-dd") +
                                    "&PersonSnils_Snils=" + person.SnilsPerson
                                    );
            jsondoc = JsonDocument.Parse(response);
            JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
            if (error_code.GetInt32() == 0)
            {
                JsonElement data = jsondoc.RootElement.GetProperty("data");
                if (data.GetArrayLength()!=0)
                idperson = Convert.ToInt32( data[0].GetProperty("Person_id").GetString());
            }

            return idperson;
        }
        public int savePerson(Person person)
        {
            int idperson = -1; error_msg = "";
            DateTime birthDayPerson = (person.birthDayPerson == null ? DateTime.Now.Date : (DateTime)person.birthDayPerson);
            response = SendPost("/api/Person","Sess_id=" + sess_id +
                                      "&PersonSurName_SurName=" + person.FamilyPerson +
                                    "&PersonFirName_FirName=" + person.NamePerson +
                                    "&PersonSecName_SecName=" + person.FathersPerson +
                                    "&PersonBirthDay_BirthDay=" + birthDayPerson.ToString("yyyy-MM-dd") +
                                    "&PersonSnils_Snils=" + person.SnilsPerson
                                    );
            jsondoc = JsonDocument.Parse(response);
            JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
            if (error_code.GetInt32() == 0)
            {
                JsonElement data = jsondoc.RootElement.GetProperty("data");
                if (data.GetArrayLength() != 0)
                    idperson = Convert.ToInt32(data[0].GetProperty("Person_id").GetString());
            }
            else
            {
                JsonElement error_message = jsondoc.RootElement.GetProperty("error_msg");
                error_msg = error_message.GetString();
            }

            return idperson;
        }

        // Паспорт
         public bool GetPasport(int Person_id)
        {
            response = SendGet("/api/Document?Person_id="+Person_id
                                    );
            jsondoc = JsonDocument.Parse(response);
            JsonElement error_code = jsondoc.RootElement.GetProperty("error_code");
            if (error_code.GetInt32() == 0)
            {
                JsonElement data = jsondoc.RootElement.GetProperty("data");
                if (data.GetArrayLength() != 0) {
                    //idperson = Convert.ToInt32(data[0].GetProperty("Person_id").GetString());
                }
            }

            return true;
        }

    }
}
