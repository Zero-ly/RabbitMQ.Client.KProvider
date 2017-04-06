using K.Mq.BaseTests;
using K.MqClient.Tests;
using NUnit.Framework;
using RabbitMQ.Client.KProvider;
using RabbitMQ.Client.KProvider.Configuration;

namespace K.MqClient.Tests.Fanouts
{
    public class SubscriptPublisherTests : BaseTests
    {
        #region Fileds
        //protected Sender _sender;
        protected IPublisher _sender;
        #endregion

        [SetUp]
        public void SetUp()
        {
            base.Setup();
            var mqCofnig = new MqConfig(config.Host, config.Port, config.Username, config.Password, config.VirtualHost);
            _sender = new SubscriptPublisher(mqCofnig);
        }

        [Test]
        public void Can_send_string()
        {
            _sender.Send("k_mq_string", "string_小明");
        }

        [Test]
        public void Can_send_base_object()
        {
            var student = new Student { Id = 1, Name = "base_小明" };
            _sender.Send(student);
        }

        [Test]
        public void Can_send_diff_object()
        {
            var p = new Person { Id = 100, Firstname = "diff_attribute_小明" };
            _sender.Send(p);
        }

        [Test]
        public void Can_send_notbase_same_object()
        {
            var p = new People { Id = 1000, Name = "same_attribute_小明" };
            _sender.Send(p);
        }
    }
}
