using System;

namespace RabbitMQ.Client.KProvider
{
    public interface IPublisher : IDisposable
    {
        void SendBytes(string key, byte[] msg);
    }
}
