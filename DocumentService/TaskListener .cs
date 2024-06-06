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
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Mail;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Telegram.Bot.Polling;
using System.Threading;
using System.Collections;
using DocumentService.Repositories.Interfaces;
using DocumentService.Repositories.Entities;


namespace DocumentService
{
    public class TaskListener : IHostedService
    {
        private readonly ISubscriberTask subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private readonly IServer server;
        private readonly Promed promed;
        private readonly Telegram.Bot.TelegramBotClient botClient;
        private readonly ItelegramChat? itelegramChat;

        public TaskListener(ISubscriberTask subscriber, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, IServer server) //  , ItelegramChatId itelegramChatId  , IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
            this.server = server;
            this.promed = new Promed(false, configuration.GetConnectionString("cifromedLogin"), configuration.GetConnectionString("cifromedPassword"));
            ////this.itelegramChat = itelegramChat;
            var botToken = configuration.GetConnectionString("TELEGRAMTOKEN"); // "7131444788:AAF_IlCT5LDWKhHq8ygOzZE2lShPwPSN_dU";
            this.botClient = new Telegram.Bot.TelegramBotClient(botToken);
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };
            using CancellationTokenSource cts = new();
            botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

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
                        dbContext.Entry(UserTask).Property("successfully").CurrentValue = responsedoc.FioFinish != null;
                        dbContext.SaveChanges();

                    }


                        //UserTask.IdFio = person.IdPromedPerson;
                        //dbContext.SaveChanges();
                        if (UserTask.TextTask == null)
                            return true;

                        if (!UserTask.successfully)
                        {
                            ICollection<string> addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
                            string AppPath = addresses.FirstOrDefault();
                            string LocalPath = AppContext.BaseDirectory;

                            Console.WriteLine(" uri " + AppPath + "/Mail?UserTaskJSON=" + "\n");
                            AppPath = @"http://Localhost:23762/";
                            if (AppPath.EndsWith(@"/"))
                            { AppPath = AppPath.Remove(AppPath.Length - 1); }

                            foreach (var address in addresses)
                            {
                                Errors = Errors + " " + address; // AppPath + "/Mail?UserTaskJSON=" + JsonConvert.SerializeObject(UserTask);
                            }
                            /// Convert.ToDateTime(UserTask.DataFinish);
                            /// 
                            string addprm = JsonConvert.SerializeObject(UserTask);
                            addprm = addprm.Replace("03:00", "");
                            string BodyHTML = GetAsync(AppPath + "/Mail?UserTaskJSON=" + addprm);

                            string addtomessage = "";

                            try
                            {

                                if (UserTask.usetelegram == null) { UserTask.usetelegram = false; };
                                if ((bool)UserTask.usetelegram)
                                {
                                    addtomessage = addtomessage + " Отправка телеграм ";
                                    Console.WriteLine(" Отправка телеграм " + "\n");

                                    //var me = botClient.GetMeAsync().Result;
                                    //var upd = botClient.GetUpdatesAsync().Result;

                                    //    ///UserTask.telegram = "vologdakoneva";

                                    //    //Update? update = upd.FirstOrDefault(p => p.Message.Chat.Username.ToUpper() == UserTask.telegram.Replace("@", "").ToUpper());
                                    //    Update? update = new Update();
                                    //    foreach (var item in upd)
                                    //    {
                                    //        if (item.Message.Chat.Username != null && item.Message.Chat.Username.ToUpper() == UserTask.telegram.Replace("@", "").ToUpper())
                                    //        { update = item; break; }
                                    //    } 


                                    ///if (update != null)
                                    telegramChat? telegramChat = dbContext.ChatId.Where(p => p.Username.ToLower() == UserTask.telegram.Replace("@", "").ToLower()).FirstOrDefault();

                                    if (telegramChat != null)
                                    {
                                        addtomessage = addtomessage + " Чат найден ";
                                        string Telegamesage = UserTask.DataTask.ToString("dd-MM-yyy") + " " + UserTask.ownertask + " назначил Вам задачу \n" +
                                                              "<b>" + UserTask.TextTask + "</b>" + "\n" +
                                                              "Срок исполнения " + UserTask.DataTaskPlan.ToString("dd-MM-yyy") + "\n" +
                                                              " Срочность: " + (UserTask.PriorityTask == "2" ? "в порядке очереди" : "первоочередная");



                                        botClient.SendTextMessageAsync(telegramChat.ChatId, Telegamesage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                addtomessage = ex.Message;
                            }

                            if (UserTask.usemail == null) { UserTask.usemail = false; };
                            if ((bool)UserTask.usemail)
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
                        else {
                            
                                if (UserTask.usetelegram == null) { UserTask.usetelegram = false; };
                                if ((bool)UserTask.usetelegram)
                                {
                                telegramChat? telegramChat = dbContext.ChatId.Where(p => p.Username.ToLower() == UserTask.telegram.Replace("@", "").ToLower()).FirstOrDefault();

                                if (telegramChat != null)
                                {
                                    string Telegamesage = UserTask.FioExec+ " \n" + 
                                                          "Задача завершена.  " + UserTask.TextTask + " \n Спасибо.";



                                    botClient.SendTextMessageAsync(telegramChat.ChatId, Telegamesage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                                }
                            
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

        private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            return; 
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            ///start
            
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();
                telegramChat? telegramChat = dbContext.ChatId.Where(p => p.Username.ToLower() == message.Chat.Username.ToLower()).FirstOrDefault();

                if (telegramChat==null )
                {
                    telegramChat telegramChatone = new telegramChat();
                    telegramChatone.Username = message.Chat.Username;
                    telegramChatone.ChatId = message.Chat.Id.ToString();
                    dbContext.Entry(telegramChatone).State = EntityState.Modified;
                    dbContext.ChatId.Attach(telegramChatone);
                    dbContext.ChatId.Add(telegramChatone);
                    dbContext.SaveChanges();
                }
                if (message.Text.ToLower() == "/start")
                {
                    string Telegamesage = " Добро пожаловать  \n" +
                                            "в чат ЧГП № 7";
                    await botClient.SendTextMessageAsync(chatId, Telegamesage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
            }
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
