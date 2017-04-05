using System;
using System.Configuration;
using System.Xml;

namespace RabbitMQ.Client.KProvider.Configuration
{
    public class MqConfig : IConfigurationSectionHandler
    {
        public const int DEFAULT_PORT = 5672;
        public const string DEFAULT_CONFIG_NAME = "K.RabbitMQ";

        #region Fields
        public int Port { get; private set; }
        public string Host { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string VirtualHost { get; private set; }
        #endregion

        #region Constructor
        public MqConfig() { }
        public MqConfig(string host, int port = DEFAULT_PORT, string username = null, string password = null, string virtualHost = "")
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            VirtualHost = virtualHost;
        }
        #endregion


        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new MqConfig();
            var uriNode = section.SelectSingleNode("Uri");
            if (uriNode != null && uriNode.Attributes != null)
            {
                var attribute = uriNode.Attributes["Host"];
                if (attribute != null)
                    config.Host = attribute.Value;

                int port = DEFAULT_PORT;
                var attribute2 = uriNode.Attributes["Port"];
                if (attribute2 != null)
                    Int32.TryParse(attribute2.Value, out port);
                config.Port = port;

                var attribute3 = uriNode.Attributes["VirtualHost"];
                if (attribute3 != null)
                    config.VirtualHost = attribute3.Value;
            }

            var authNode = section.SelectSingleNode("Authentication");
            if (authNode != null && authNode.Attributes != null)
            {
                var attribute = authNode.Attributes["Username"];
                if (attribute != null)
                    config.Username = attribute.Value;

                var attribute2 = authNode.Attributes["Password"];
                if (attribute2 != null)
                    config.Password = attribute2.Value;
            }

            return config;
        }

    }
}
