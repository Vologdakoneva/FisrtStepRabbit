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
    public class DocAnalizItems : ControllerBase
    {
        private readonly IConfiguration configuration;

        public DocAnalizItems(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public DocItems[] Get()
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
                DocItems[] items;
                    items = JsonConvert.DeserializeObject<DocItems[]>(docAnaliz.Items);
                    if (items == null)
                        items = new DocItems[0];
                    return items;
            }
            catch (Exception)
            {
                DocItems[] docAnalizs = new DocItems[1];
                docAnalizs[0] = new DocItems() { AnalizText = "Ошибка сервиса Документы" };
                return docAnalizs;

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