using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.KProvider.Configuration;

namespace RabbitMQ.Client.KProvider
{
    /// <summary>
    /// 接收者（任务模式）
    /// </summary>
    /// <remarks>
    /// 注：
    ///     这里不使用Mq中的队列(queue)或交换器(exchange)；
    ///     我们将使用key(做为消息匹配键)；
    ///     在同一种模式下，使用键值匹配发送消息和监听消息。
    /// </remarks>
    public class TaskConsumer : IConsumer
    {
        #region Fields
        //private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly MqConfig _config;
        #endregion

        #region Constructor
        public TaskConsumer() : this(System.Configuration.ConfigurationManager.GetSection(MqConfig.DEFAULT_CONFIG_NAME) as MqConfig) { }
        public TaskConsumer(string host, int port, string username = null, string password = null, string virtualHost = "")
            : this(new MqConfig(host: host, port: port, username: username, password: password, virtualHost: virtualHost)) { }
        public TaskConsumer(MqConfig config)
        {
            _config = config;

            var factory = new ConnectionFactory { HostName = _config.Host, Port = _config.Port, UserName = _config.Username, Password = _config.Password, VirtualHost = _config.VirtualHost };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        #endregion

        #region Methods
        public void ReceiveBytes(string key, Action<byte[]> action)
        {
            channel.QueueDeclare(queue: key, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                action(ea.Body);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
            };
            channel.BasicConsume(queue: key, noAck: false, consumer: consumer);

        }
        #endregion

        #region Dispose
        ~TaskConsumer()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.IsOpen)
                    connection.Close();
                connection.Dispose();
                //connection = null;
            }

            if (channel != null)
            {
                if (channel.IsOpen)
                    channel.Close();
                channel.Dispose();
                //channel = null;
            }
        }
        #endregion
    }
}
