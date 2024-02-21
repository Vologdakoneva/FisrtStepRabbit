using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PacientService.Controllers;
using PacientService.Data;
using PacientService.Entities;
//using PacientService.Migrations;
using PacientService.Repositories.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;
using PromedExchange;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace PacientService
{
    public class PacientListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Promed promed = new Promed(false);


        public PacientListener(ISubscriber subscriber, IServiceScopeFactory serviceScopeFactory) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;

        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private readonly object SubscribeLock = new object();
        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            Console.WriteLine(" Сообщение получено " + "\n");
            Thread.BeginCriticalRegion();
            try
            {


            PacientService.Entities.Person response;
            try
            {
                response = JsonConvert.DeserializeObject<PacientService.Entities.Person>(message);
            }
            catch (Exception)
            {

                Console.WriteLine(" Сообщение ошибка данных " + "\n");
                return true;
            }
            if (response != null)
                Console.WriteLine(" Сообщение начата обработка " + "\n");
            try
            {

            using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PacientDbContext>();

                    if (response.FathersPerson == "")
                    {
                        PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указано отчество" + response.NamePerson + " " + response.FamilyPerson + " " ;
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }
                    if (response.NamePerson == "")
                    {
                        PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указано имя пацинта" +  response.FamilyPerson + " " + response.FathersPerson;
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }
                    if (response.SnilsPerson == "" || response.SnilsPerson == null)
                    {
                        PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указан СНИЛС пацинта " + response.NamePerson + " " + response.FamilyPerson + " " + response.FathersPerson;
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }

                    PacientService.Entities.Person? person = dbContext.Person.Where(p => p.PersonLink == response.PersonLink).FirstOrDefault();
                    if (person == null || person.PersonLink != response.PersonLink)
                    {
                        dbContext.Person.Add(response);
                        dbContext.SaveChanges();
                        person = dbContext.Person.Where(p => p.PersonLink == response.PersonLink).FirstOrDefault();
                    }
                    else
                    {
                        var entityProperties = person.GetType().GetProperties();
                        dbContext.Entry(person).State = EntityState.Modified;
                        dbContext.Person.Attach(person);
                        response.DateChangePerson = DateTime.Now;
                        foreach (var ep in entityProperties)
                        {
                            if (ep.Name != "IDALL" && ep.Name != "DataCreatePerson") {
                                dbContext.Entry(person).Property(ep.Name).IsModified = true;
                                dbContext.Entry(person).Property(ep.Name).CurrentValue = dbContext.Entry(response).Property(ep.Name).CurrentValue;
                            }
                        }                        //response.IDALL = person.IDALL;
                                                 //person = response;
                                                 //dbContext.Entry(person).CurrentValues.SetValues(response);
                                                 //dbContext.Entry(person).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }


                    Console.WriteLine(" SaveChanges " + "\n");

                    Console.WriteLine(" GetPerson " + "\n");
                    Int64 idperson = promed.GetPerson(ConvertPerson(response));  ////promed.SendPut("","");


                    if (idperson == -10)
                    {
                        Console.WriteLine(" Login Failed Promed " + "\n");
                        PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Логин не успешный" ;
                        errorPerson.ErrorSource = "Промед";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return  true;

                    }

                    // Добавить пациента
                    if (idperson == -1) {
                        idperson = promed.savePerson(ConvertPerson(response));
                    }

                    if (idperson != -1) {
                        if (response.SeriaDoc == "" || response.NomDoc == "")
                        {
                            PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                            errorPerson.PersonLink = response.PersonLink;
                            errorPerson.ErrorText = "Не указан паспорт" + response.NamePerson + " " + response.FamilyPerson + " " + response.FathersPerson;
                            errorPerson.ErrorSource = "1С";
                            dbContext.ErrorPerson.Add(errorPerson);
                            dbContext.SaveChanges();

                        }
                        else  // Работа с паспортом
                        {

                        }

                        dbContext.Entry(person).Property("IdPromedPerson").IsModified = true;
                        dbContext.Entry(person).Property("IdPromedPerson").CurrentValue = idperson;
                        dbContext.Entry(person).Property("successfully").IsModified = true;
                        dbContext.Entry(person).Property("successfully").CurrentValue = true;
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        PacientService.Entities.ErrorPerson errorPerson = new PacientService.Entities.ErrorPerson();
                        errorPerson.PersonLink = person.PersonLink;
                        errorPerson.ErrorText = promed.error_msg;
                        errorPerson.ErrorSource = "Промед";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Сообщение error " + ex.Message + "\n");

            }
            }
            finally
            {
                Thread.EndCriticalRegion();
            }
            Console.WriteLine(" Сообщение обработано успешно " + DateTime.Now.ToString() + "\n");

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public PromedExchange.Person ConvertPerson(PacientService.Entities.Person person) {
               
            PromedExchange.Person result = new PromedExchange.Person();


            foreach (PropertyInfo property in typeof(PacientService.Entities.Person).GetProperties().Where(p => p.CanWrite))
            {
                var objectset = typeof(PromedExchange.Person).GetProperties().FirstOrDefault(p => p.Name == property.Name);
                objectset.SetValue(result, property.GetValue(person));


                //property.SetValue(objectset, property.GetValue(person, null), null);
            }
            return result;
        }
         
    }
}
