using System;
using K.MqConsumer.Tests.Subscription;
using K.MqConsumer.Tests.Tasks;

namespace K.MqConsumer.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sub = new SubscriptConsumerTests();
                sub.Can_send_string();
                sub.Can_send_base_object();
                sub.Can_send_diff_object();
                sub.Can_send_notbase_same_object();

                var task = new TaskConsumerTests();
                task.Can_send_string();
                task.Can_send_base_object();
                task.Can_send_diff_object();
                task.Can_send_notbase_same_object();
                task.Can_unack_when_excepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
