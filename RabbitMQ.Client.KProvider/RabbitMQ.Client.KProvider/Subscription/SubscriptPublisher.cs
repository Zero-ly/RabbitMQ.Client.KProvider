using RabbitMQ.Client.Content;
using RabbitMQ.Client.KProvider.Configuration;

namespace RabbitMQ.Client.KProvider
{
    /// <summary>
    /// 发送者（广播/订阅模式）
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SubscriptPublisher : IPublisher
    {
        //private const string DEFAULT_EXCHANGE_NAME = "k.ex1";
        #region Fields
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly MqConfig _config;
        #endregion

        #region Constructor
        public SubscriptPublisher() : this(System.Configuration.ConfigurationManager.GetSection(MqConfig.DEFAULT_CONFIG_NAME) as MqConfig) { }
        public SubscriptPublisher(string host, int port, string username = null, string password = null, string virtualHost = "")
            : this(new MqConfig(host: host, port: port, username: username, password: password, virtualHost: virtualHost)) { }
        public SubscriptPublisher(MqConfig config)
        {
            _config = config;

            var factory = new ConnectionFactory { HostName = _config.Host, Port = _config.Port, UserName = _config.Username, Password = _config.Password, VirtualHost = _config.VirtualHost };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            properties = (IBasicProperties)(new MapMessageBuilder(channel).GetContentHeader());
            properties.DeliveryMode = 2;
        }
        #endregion

        #region Sends
        /// <summary>
        /// 发送二进制信息
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="bMessage">二进制信息</param>
        public void SendBytes(string key, byte[] bMessage)
        {
            channel.ExchangeDeclare(exchange: key, type: ExchangeType.Fanout);
            channel.BasicPublish(exchange: key, routingKey: "", basicProperties: properties, body: bMessage);
        }
        #endregion

        #region Dispose
        ~SubscriptPublisher()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (connection != null)
            {
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
