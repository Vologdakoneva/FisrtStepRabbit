using DocumentService.Data;
using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace DocumentService
{
    public class NaAnalizListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public NaAnalizListener(ISubscriber subscriber, IServiceScopeFactory serviceScopeFactory) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;

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
