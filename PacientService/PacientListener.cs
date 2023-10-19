using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace PacientService
{
    public class PacientListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IPacientOperator pacientOperator;

        public PacientListener(ISubscriber subscriber) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            //this.pacientOperator = pacientOperator;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            //var response = JsonConvert.DeserializeObject<InventoryResponse>(message);
            //if (!response.IsSuccess)
            //{
            //    //orderDeletor.Delete(response.OrderId).GetAwaiter().GetResult();
            //}
            Thread.Sleep(40000);
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
