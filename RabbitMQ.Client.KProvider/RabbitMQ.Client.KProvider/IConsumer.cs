using System;

namespace RabbitMQ.Client.KProvider
{
    public interface IConsumer : IDisposable
    {
        void ReceiveBytes(string key, Action<byte[]> action);
    }
}
