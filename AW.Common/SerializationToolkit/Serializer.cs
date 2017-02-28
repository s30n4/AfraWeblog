using Newtonsoft.Json;

namespace AW.Common.SerializationToolkit
{
    public static class Serializer
    {
        public static string JsonSerialize<T>(T obj)
        {
            var result = JsonConvert.SerializeObject(obj);
            return result;
        }

        public static T JsonDeserialize<T>(string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}
