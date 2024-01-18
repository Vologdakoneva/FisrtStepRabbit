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
    public class DocAnalizErrors : ControllerBase
    {
        private readonly IConfiguration configuration;

        public DocAnalizErrors(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public DocError[] Get()
        {
            bool isErr = false;
            string DocLink = HttpUtility.ParseQueryString(Request.QueryString.Value).Get("DocLink");
            if (DocLink==null) { 
                DocLink = HttpUtility.ParseQueryString(Request.QueryString.Value).Get("DocLinkERR");
                isErr = true;
            }

            try
            {
                string urlDocService = configuration.GetConnectionString("DocumentService");
                string Respose = GetAsync(urlDocService+"/"+ DocLink);
                DocAnaliz docAnaliz = JsonConvert.DeserializeObject<DocAnaliz>(Respose);
                DocError[] docErrors;
                    docErrors = JsonConvert.DeserializeObject<DocError[]>(docAnaliz.Errors);
                if (docErrors==null) docErrors = new DocError[0];
                return docErrors;
            }
            catch (Exception)
            {
                DocError[] docErrors = new DocError[1];
                docErrors[0] = new DocError() { ErrorText = "Ошибка сервиса Документы" };
                return docErrors;

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