using System;
using System.Threading;
using K.Mq.BaseTests;
using K.MqClient.Tests;
using K.MqServer.Tests;
using RabbitMQ.Client.KProvider;
using RabbitMQ.Client.KProvider.Configuration;

namespace K.MqConsumer.Tests.Tasks
{
    public class TaskConsumerTests : BaseTests
    {
        private const int SLEEP_TIME = 100;
        #region Fileds
        //protected Receiver _receiver;
        protected IConsumer _receiver;
        #endregion

        public TaskConsumerTests()
        {
            base.Setup();
            var mqCofnig = new MqConfig(config.Host, config.Port, config.Username, config.Password, config.VirtualHost);
            _receiver = new TaskConsumer(mqCofnig);
        }


        public void Can_send_string()
        {
            _receiver.Receive("k_mq_string", e =>
            {
                Console.WriteLine(string.Format("【Task】 receive:{0}", e));
                Thread.Sleep(SLEEP_TIME);
            });
        }


        public void Can_send_base_object()
        {
            _receiver.Receive<Student>(e =>
            {
                Console.WriteLine(
                    string.Format("【Task】 receive:Id={0},Name={1}", e.Id, e.Name));
                Thread.Sleep(SLEEP_TIME);
            });
        }


        public void Can_send_diff_object()
        {
            _receiver.Receive<Person>(e =>
            {
                Console.WriteLine(
                    string.Format("【Task】 receive:Id={0},Firstname={1}", e.Id, e.Firstname));
                Thread.Sleep(SLEEP_TIME);
            });
        }


        public void Can_send_notbase_same_object()
        {
            _receiver.Receive<People>(e =>
            {
                Console.WriteLine(
                    string.Format("【Task】 receive:Id={0},Firstname={1}", e.Id, e.Name));
                Thread.Sleep(SLEEP_TIME);
            });
        }
    }
}
