using System;
using System.Configuration;
using RabbitMQ.Client.KProvider.Configuration;

namespace K.MqClient.Tests
{
    public abstract class BaseTests
    {
        protected MqConfig config { get; set; }
        public void Setup()
        {
            //AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "App.config");
            config = ConfigurationManager.GetSection(MqConfig.DEFAULT_CONFIG_NAME) as MqConfig;
            if (config == null) config = new MqConfig();
        }
    }
}
