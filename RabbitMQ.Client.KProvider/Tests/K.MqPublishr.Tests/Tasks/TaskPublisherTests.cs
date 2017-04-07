using K.Mq.BaseTests;
using NUnit.Framework;
using RabbitMQ.Client.KProvider;
using RabbitMQ.Client.KProvider.Configuration;

namespace K.MqClient.Tests.Tasks
{
    public class TaskPublisherTests : BaseTests
    {
        #region Fileds
        //protected Sender _sender;
        protected IPublisher _sender;
        #endregion

        [SetUp]
        public void SetUp()
        {
            base.Setup();
            //_sender = new Sender(config);
            var mqCofnig = new MqConfig(config.Host, config.Port, config.Username, config.Password, config.VirtualHost);
            _sender = new TaskPublisher(mqCofnig);
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

        [Test]
        public void Can_unack_when_excepted()
        {
            for (var i = 0; i < 20; i++)
            {
                _sender.Send("k_mq_uncak_when_excepted", i);
            }
        }

        /// <summary>
        /// 先发送，后启动接收者，是否会只分派给一个接收者？
        /// 答案是：否,?
        /// </summary>
        [Test]
        public void Is_task_on_same_time_start_up_tests()
        {
            for (var i = 0; i < 10; i++)
            {
                _sender.Send("k_mq_string", "string_小明___" + i);
            }
        }
    }
}
