using Newtonsoft.Json;

namespace CQRS.Core
{
    public class MySerializer : IMySerializer
    {
        public string Serializ(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}