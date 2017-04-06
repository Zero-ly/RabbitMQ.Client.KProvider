using System;
using K.Mq.BaseTests;
using K.MqClient.Tests;
using K.MqServer.Tests;
using RabbitMQ.Client.KProvider;
using RabbitMQ.Client.KProvider.Configuration;

namespace K.MqConsumer.Tests.Subscription
{
    public class SubscriptConsumerTests : BaseTests
    {
        #region Fileds
        //protected Receiver _receiver;
        protected IConsumer _receiver;
        #endregion

        public SubscriptConsumerTests()
        {
            base.Setup();
            var mqCofnig = new MqConfig(config.Host, config.Port, config.Username, config.Password, config.VirtualHost);
            _receiver = new SubscriptConsumer(mqCofnig);
        }


        public void Can_send_string()
        {
            _receiver.Receive("k_mq_string", e => { Console.WriteLine(string.Format("[Subscript] receive:{0}", e)); });
        }


        public void Can_send_base_object()
        {
            _receiver.Receive<Student>(e =>
            {
                Console.WriteLine(
                    string.Format("[Subscript] receive:Id={0},Name={1}", e.Id, e.Name));
            });
        }


        public void Can_send_diff_object()
        {
            _receiver.Receive<Person>(e =>
            {
                Console.WriteLine(
                    string.Format("[Subscript] receive:Id={0},Firstname={1}", e.Id, e.Firstname));
            });
        }


        public void Can_send_notbase_same_object()
        {
            _receiver.Receive<People>(e =>
            {
                Console.WriteLine(
                    string.Format("[Subscript] receive:Id={0},Firstname={1}", e.Id, e.Name));
            });
        }
    }
}
