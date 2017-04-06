using RabbitMQ.Client.Content;
using RabbitMQ.Client.KProvider.Configuration;

namespace RabbitMQ.Client.KProvider
{
    /// <summary>
    /// 发送者（任务模式）
    /// </summary>
    /// <remarks>
    /// 注1：
    ///     均默认为持久化、不自动删除（DeliveryMode = 2;durable: true, exclusive: false, autoDelete: false）
    /// 注2：
    ///     这里不使用Mq中的队列(queue)或交换器(exchange)；
    ///     我们将使用key(做为消息匹配键)；
    ///     在同一种模式（任务模式/订阅模式）下，使用键值匹配发送消息和监听消息。
    /// </remarks>
    public class TaskPublisher : IPublisher
    {
        #region Fields
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly MqConfig _config;
        #endregion

        #region Constructor
        public TaskPublisher() : this(System.Configuration.ConfigurationManager.GetSection(MqConfig.DEFAULT_CONFIG_NAME) as MqConfig) { }
        public TaskPublisher(string host, int port, string username = null, string password = null, string virtualHost = "")
            : this(new MqConfig(host: host, port: port, username: username, password: password, virtualHost: virtualHost)) { }
        public TaskPublisher(MqConfig config)
        {
            _config = config;

            var factory = new ConnectionFactory { HostName = _config.Host, Port = _config.Port, UserName = _config.Username, Password = _config.Password, VirtualHost = _config.VirtualHost };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            //properties.DeliveryMode = 2;  //持久化 另一种设置方式
        }
        #endregion

        #region Sends
        /// <summary>
        /// 发送二进制信息
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="msg">二进制信息</param>
        public void SendBytes(string key, byte[] msg)
        {
            //ToImprove 减少固定参数
            channel.QueueDeclare(queue: key, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: key, basicProperties: properties, body: msg);
        }
        #endregion

        #region Dispose
        ~TaskPublisher()
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
                channel.Close();
                channel.Dispose();
                //channel = null;
            }
        }
        #endregion
    }
}
