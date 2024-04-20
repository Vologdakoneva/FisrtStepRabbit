using DocumentService.Data;
using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using static DocumentService.Entities.Docsummary;
using PromedExchange;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Telegram.Bot;
using System.Net.Mail;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace DocumentService
{
    public class TaskListener : IHostedService
    {
        private readonly ISubscriberTask subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private readonly IServer server;
        private readonly Promed promed;

        public TaskListener(ISubscriberTask subscriber, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, IServer server) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
            this.server = server;
            this.promed = new Promed(false, configuration.GetConnectionString("cifromedLogin"), configuration.GetConnectionString("cifromedPassword"));

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(SubscribeTask);
            return Task.CompletedTask;
        }

        private bool SubscribeTask(string message, IDictionary<string, object> dictionary)
        {
            Console.WriteLine(" Сообщение получено " + DateTime.Now, ToString() + "\n");

            //var botToken = "7131444788:AAF_IlCT5LDWKhHq8ygOzZE2lShPwPSN_dU";
            //Telegram.Bot.TelegramBotClient botClient = new Telegram.Bot.TelegramBotClient(botToken);

            //var me = botClient.GetMeAsync().Result;
            //var upd = botClient.GetUpdatesAsync().Result;
            //botClient.SendTextMessageAsync(me.Id, "ss");




            Thread.BeginCriticalRegion();
            string Errors = "";
            try
            {
                try
                {


                UserTasks responsedoc;
                try
                {
                    responsedoc = JsonConvert.DeserializeObject<UserTasks>(message);
                }
                catch (Exception)
                {

                    Console.WriteLine(" Сообщение ошибка данных " + "\n");
                    return true;
                }

                if (responsedoc == null) { return true; }
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();

                    UserTasks? UserTask = dbContext.UserTask.Where(p => p.DocLink == responsedoc.DocLink).FirstOrDefault();
                    if (UserTask == null)
                    {
                        UserTask = responsedoc;
                        dbContext.UserTask.Add(UserTask);
                        dbContext.SaveChanges();
                        UserTask = dbContext.UserTask.Where(p => p.DocLink == responsedoc.DocLink).FirstOrDefault();
                    }
                    else
                    {
                        var entityProperties = UserTask.GetType().GetProperties();
                        dbContext.Entry(UserTask).State = EntityState.Modified;
                        dbContext.UserTask.Attach(UserTask);

                        foreach (var ep in entityProperties)
                        {
                            if (ep.Name != "IDALL" && ep.Name != "DataCreatePerson")
                            {
                                //dbContext.Entry(UserTask.DateChangePerson = DateTime.Now;).Property(ep.Name).IsModified = true;
                                dbContext.Entry(UserTask).Property(ep.Name).CurrentValue = dbContext.Entry(responsedoc).Property(ep.Name).CurrentValue;
                            }
                        }                        //response.IDALL = person.IDALL;
                                                 //person = response;
                                                 //dbContext.Entry(person).CurrentValues.SetValues(response);
                                                 //dbContext.Entry(person).State = EntityState.Modified;
                        dbContext.Entry(UserTask).Property("successfully").CurrentValue = responsedoc.DataFinish != null;
                        dbContext.SaveChanges();

                    }


                    //UserTask.IdFio = person.IdPromedPerson;
                    //dbContext.SaveChanges();


                    if (!UserTask.successfully)
                    {
                        ICollection<string> addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
                        string AppPath = addresses.FirstOrDefault();
                        string LocalPath = AppContext.BaseDirectory;

                            Console.WriteLine(" uri " +  AppPath + "/Mail?UserTaskJSON=" +"\n");
                            AppPath = @"http://Localhost:23762/";
                            if (AppPath.EndsWith(@"/")) 
                            { AppPath = AppPath.Remove(AppPath.Length - 1); }

                            foreach (var address in addresses) {
                                Errors = Errors + " " +address; // AppPath + "/Mail?UserTaskJSON=" + JsonConvert.SerializeObject(UserTask);
                                 }

                                string BodyHTML = GetAsync(AppPath + "/Mail?UserTaskJSON=" + JsonConvert.SerializeObject(UserTask));

                            string addtomessage = "";

                            try
                            {

                            if (UserTask.usetelegram == null) { UserTask.usetelegram = false; };
                            if ((bool)UserTask.usetelegram)
                            {
                                addtomessage = addtomessage + " Отправка телеграм ";
                                Console.WriteLine(" Отправка телеграм " + "\n");
                                var botToken = configuration.GetConnectionString("TELEGRAMTOKEN"); // "7131444788:AAF_IlCT5LDWKhHq8ygOzZE2lShPwPSN_dU";
                                Telegram.Bot.TelegramBotClient botClient = new Telegram.Bot.TelegramBotClient(botToken);

                                var me = botClient.GetMeAsync().Result;
                                var upd = botClient.GetUpdatesAsync().Result;

                                ///UserTask.telegram = "vologdakoneva";

                                Update? update = upd.FirstOrDefault(p => p.Message.Chat.Username.ToUpper() == UserTask.telegram.Replace("@", "").ToUpper());


                                if (update != null)
                                {
                                    addtomessage = addtomessage + " Чат найден ";
                                    string Telegamesage = UserTask.DataTask.ToString("dd-MM-yyy") + " " + UserTask.ownertask + " назначил Вам зачачу \n" +
                                                          "<b>" + UserTask.TextTask + "</b>" + "\n" +
                                                          "Срок исполнения " + UserTask.DataTaskPlan.ToString("dd-MM-yyy") + "\n" +
                                                          " Срочность: " + (UserTask.PriorityTask == "2" ? "в порядке очереди" : "первоочередная");



                                    botClient.SendTextMessageAsync(update.Message.Chat.Id, Telegamesage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                                }

                            }
                            }
                            catch (Exception ex)
                            {
                                addtomessage = ex.Message;
                            }

                            if (UserTask.usemail == null) { UserTask.usemail = false; };
                        if ((bool)UserTask.usemail )
                        {
                                Console.WriteLine(" Отправка майл " + "\n");
                                SmtpClient smtpClient = new SmtpClient(configuration.GetConnectionString("SMTP"), 25); //25 465

                            smtpClient.Credentials = new System.Net.NetworkCredential(configuration.GetConnectionString("SMTPUSER"), configuration.GetConnectionString("SMTPPASSWORD"));
                            //smtpClient.UseDefaultCredentials = true;
                            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.EnableSsl = true;
                            MailMessage mail = new MailMessage();
                            mail.IsBodyHtml = true;


                            mail.Body = BodyHTML; // BodyHTML;  

                                
                                

                                //Setting From , To and CC
                                mail.From = new MailAddress(configuration.GetConnectionString("SMTPUSER"), "Администратор");
                                mail.To.Add(new MailAddress(UserTask.email));
                                mail.Subject = "Задача"; //configuration.GetConnectionString("SMTPSUBJECT");
                            

                                smtpClient.Send(mail);
                            mail.Dispose();
                            smtpClient.Dispose();
                        }
                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine( ee.Message +  "\n");
                    string LocalPath = AppContext.BaseDirectory;
                    using (StreamWriter writetext = new StreamWriter(LocalPath+"/write.txt"))
                    {
                        writetext.WriteLine(ee.Message + "  "  + Errors);
                    }
                }
            }
            finally
            {
                Thread.EndCriticalRegion();

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
