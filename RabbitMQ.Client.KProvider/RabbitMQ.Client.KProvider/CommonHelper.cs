using System.Text;
using Newtonsoft.Json;

namespace RabbitMQ.Client.KProvider
{
    internal class CommonHelper
    {
        #region Deserialize
        public static byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public static T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        #endregion
    }
}
