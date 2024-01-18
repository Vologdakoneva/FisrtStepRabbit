using DocumentService.Data;
using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using static DocumentService.Entities.Docsummary;
using PromedExchange;
using System.Text.Json;

namespace DocumentService
{
    public class NaAnalizListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;

        public NaAnalizListener(ISubscriber subscriber, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(SubscribeAnaliz);
            return Task.CompletedTask;
        }

        private bool SubscribeAnaliz(string message, IDictionary<string, object> dictionary)
        {
            Console.WriteLine(" Сообщение получено " + "\n");
            Docsummary responsedoc;
            try
            {
                responsedoc = JsonConvert.DeserializeObject<Docsummary>(message);
            }
            catch (Exception)
            {

                Console.WriteLine(" Сообщение ошибка данных " + "\n");
                return true;
            }
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();

                DocAnaliz? docAnaliz = dbContext.DocAnaliz.Where(p => p.DocLink == responsedoc.docAnaliz.DocLink).FirstOrDefault();
                if (docAnaliz == null)
                {
                    docAnaliz = responsedoc.docAnaliz;
                    docAnaliz.Items = JsonConvert.SerializeObject(responsedoc.Items);
                    dbContext.DocAnaliz.Add(docAnaliz);
                    dbContext.SaveChanges();
                    docAnaliz = dbContext.DocAnaliz.Where(p => p.DocLink == responsedoc.docAnaliz.DocLink).FirstOrDefault();
                }
                else
                {
                    var entityProperties = docAnaliz.GetType().GetProperties();
                    dbContext.Entry(docAnaliz).State = EntityState.Modified;
                    dbContext.DocAnaliz.Attach(docAnaliz);
                    
                    foreach (var ep in entityProperties)
                    {
                        if (ep.Name != "IDALL" && ep.Name != "DataCreatePerson")
                        {
                            //dbContext.Entry(docAnaliz.DateChangePerson = DateTime.Now;).Property(ep.Name).IsModified = true;
                            dbContext.Entry(docAnaliz).Property(ep.Name).CurrentValue = dbContext.Entry(responsedoc.docAnaliz).Property(ep.Name).CurrentValue;
                        }
                    }                        //response.IDALL = person.IDALL;
                                             //person = response;
                                             //dbContext.Entry(person).CurrentValues.SetValues(response);
                                             //dbContext.Entry(person).State = EntityState.Modified;
                    docAnaliz.DataChange = DateTime.Now;
                    dbContext.SaveChanges();

                }

                List<DocError> docErrors = new List<DocError>();
                string urlPacientService = configuration.GetConnectionString("PacientService");
                string Respose = "";
                try
                {
                    Respose = GetAsync(urlPacientService + "/" + docAnaliz.Fiokey);
                }
                catch (Exception)
                {
                    Respose = "";
                }
                Person person = JsonConvert.DeserializeObject<Person>(Respose);
                
                if (person == null || person.FamilyPerson == "")
                {
                    docErrors.Add(new DocError() { ErrorSource = "1С", ErrorText = "Пациент " + docAnaliz.Fio +" не передавался. в 1С отредактируйе его" });
                    docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                    dbContext.SaveChanges();
                    return true;
                }
                docAnaliz.IdFio = person.IdPromedPerson;
                try
                {
                    Respose = GetAsync(urlPacientService + "/" + docAnaliz.FioDoctorkey);
                }
                catch (Exception)
                {
                    Respose = "";
                }
                person = JsonConvert.DeserializeObject<Person>(Respose);
                if (person == null || person.FamilyPerson == "")
                {
                    docErrors.Add(new DocError() { ErrorSource = "1С", ErrorText = "Врач " + docAnaliz.FioDoctor + "не передавался. в 1С отредактируйе его" });
                    docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                    dbContext.SaveChanges();
                    return true;
                }
                docAnaliz.IdDoctor = person.IdPromedPerson;
                dbContext.SaveChanges();

                // Опредяем врача
                Int64 MedWorker_id = 0;
                Int64 LpuSection_id = 0;
                Int64 MedStaffFact_id = 0;
                string response = ""; JsonElement element;
                PromedExchange.Promed promed = new PromedExchange.Promed(false);
               
                    response = promed.SendGet("/api/MedWorker?Person_id=" + docAnaliz.IdDoctor);
                    if (promed.GetErrorCode()) {
                         element = promed.GetData();
                    if (element.GetArrayLength() != 0)
                    {
                        MedWorker_id = Convert.ToInt64( element[0].GetProperty("MedWorker_id").GetString() );
                    }
                    if (MedWorker_id<=0)
                    {
                        docErrors.Add(new DocError() { ErrorSource = "Промед", ErrorText = "Врач " + docAnaliz.FioDoctor + "  не определен" });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;
                    }
                    response = promed.SendGet("/api/Lpu/LpuSectionListByMO" + "?Lpu_id=" + 12600044);
                    if (promed.GetErrorCode())
                    {
                        element = promed.GetData();
                        LpuSection_id = Convert.ToInt64(element[0].GetProperty("LpuSection_id").GetString());
                    }
                    // Получение мест работы медицинского работника 
                    response = promed.SendGet("/api/MedStaffFactByMedPersonal" +
                                              "?MedPersonal_id=" + MedWorker_id +
                                              "&LpuSection_id=" + LpuSection_id);
                    if (promed.GetErrorCode())
                    {
                        element = promed.GetData();
                        MedStaffFact_id = Convert.ToInt64(element[0].GetProperty("MedStaffFact_id").GetString());
                    }
                    if ( MedStaffFact_id == 0 || LpuSection_id == 0 ) {
                        docErrors.Add(new DocError() { ErrorSource = "Промед", ErrorText = "Не указано место работы врача " + docAnaliz.FioDoctor + " не определено" });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;
                    }
                    //Создание ТАП с первым посещением для поликлинического случая


                }
                Console.WriteLine(" Сообщение обработано  " + "\n");

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
