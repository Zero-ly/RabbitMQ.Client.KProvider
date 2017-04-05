using System.Text;

namespace RabbitMQ.Client.KProvider
{
    public static class PublishrExtentions
    {
        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="message">信息内容</param>
        public static void Send(this IPublisher publisher, string key, string msg)
        {
            publisher.SendBytes(key: key, msg: Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 发送对象信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">值</param>
        public static void Send<T>(this IPublisher publisher, T t)
        {
            publisher.SendBytes(key: typeof(T).Name, msg: CommonHelper.Serialize(t));
        }

        /// <summary>
        /// 发送对象信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="publisher">发布者</param>
        /// <param name="key">键值</param>
        /// <param name="t">值</param>
        public static void Send<T>(this IPublisher publisher, string key, T t)
        {
            publisher.SendBytes(key: key, msg: CommonHelper.Serialize(t));
        }
    }
}
