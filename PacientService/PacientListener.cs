using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PacientService.Controllers;
using PacientService.Data;
using PacientService.Entities;
using PacientService.Migrations;
using PacientService.Repositories.Entities;
using PacientService.Repositories.Interfaces;
using Plain.RabbitMQ;
using PromedExchange;
using System.Drawing;
using System.Linq;

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

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            Person response = JsonConvert.DeserializeObject<Person>(message);
            if (response != null)
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PacientDbContext>();

                    if (response.FathersPerson == "" )
                    {
                        ErrorPerson errorPerson = new ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указано отчество";
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }
                    if (response.NamePerson == "")
                    {
                        ErrorPerson errorPerson = new ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указано имя пацинта";
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }
                    if (response.SnilsPerson == "")
                    {
                        ErrorPerson errorPerson = new ErrorPerson();
                        errorPerson.PersonLink = response.PersonLink;
                        errorPerson.ErrorText = "Не указан СНИЛС пацинта";
                        errorPerson.ErrorSource = "1С";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();
                        return true;
                    }

                    Person? person = dbContext.Person.Where(p => p.PersonLink == response.PersonLink).FirstOrDefault();
                if (person==null)
                {
                    dbContext.Person.Add(response);
                    dbContext.SaveChanges();
                }
                else
                    {
                        var entityProperties = person.GetType().GetProperties();
                        dbContext.Entry(person).State = EntityState.Modified;
                        dbContext.Person.Attach(person);

                            foreach (var ep in entityProperties)
                            {
                                if (ep.Name != "IDALL") {
                                dbContext.Entry(person).Property(ep.Name).IsModified = true;
                                dbContext.Entry(person).Property(ep.Name).CurrentValue = dbContext.Entry(response).Property(ep.Name).CurrentValue;
                            }
                        }                        //response.IDALL = person.IDALL;
                                                     //person = response;
                                                     //dbContext.Entry(person).CurrentValues.SetValues(response);
                                                     //dbContext.Entry(person).State = EntityState.Modified;
                            dbContext.SaveChanges();
                    }



                    int idperson = promed.GetPerson(response);  ////promed.SendPut("","");
                    // Добавить пациента
                    if (idperson == -1) {
                        idperson = promed.savePerson(response);
                    }
                    
                    if (idperson != -1) {
                        if (response.SeriaDoc == "" || response.NomDoc == "")
                        {
                            ErrorPerson errorPerson = new ErrorPerson();
                            errorPerson.PersonLink = response.PersonLink;
                            errorPerson.ErrorText = "Не указан паспорт";
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
                        ErrorPerson errorPerson = new ErrorPerson();
                        errorPerson.PersonLink = person.PersonLink;
                        errorPerson.ErrorText = promed.error_msg;
                        errorPerson.ErrorSource = "Промед";
                        dbContext.ErrorPerson.Add(errorPerson);
                        dbContext.SaveChanges();

                    }
                }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
         
    }
}
