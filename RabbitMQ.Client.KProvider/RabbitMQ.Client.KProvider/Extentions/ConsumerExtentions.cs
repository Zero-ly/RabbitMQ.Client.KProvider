using System;
using System.Text;

namespace RabbitMQ.Client.KProvider
{
    public static class ConsumerExtentions
    {
        public static void Receive(this IConsumer consumer, string key, Action<string> action)
        {
            consumer.ReceiveBytes(key: key, action: (body) => { action(Encoding.UTF8.GetString(body)); });
        }
        public static void Receive<T>(this IConsumer consumer, Action<T> action)
        {
            consumer.ReceiveBytes(key: typeof(T).Name, action: (body) => { action(CommonHelper.Deserialize<T>(body)); });
        }

        public static void Receive<T>(this IConsumer consumer, string key, Action<T> action)
        {
            consumer.ReceiveBytes(key: key, action: (body) => { action(CommonHelper.Deserialize<T>(body)); });
        }
    }
}
