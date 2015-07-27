using ServiceStack.Text;
using System;

namespace Phytel.Services.Serializer
{
    public class SerializerJson : ISerializer
    {
        public object Deserialize(string value, Type targetType)
        {
            return JsonSerializer.DeserializeFromString(value, targetType);
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.DeserializeFromString<T>(value);
        }

        public string Serialize(object value, Type type)
        {
            return JsonSerializer.SerializeToString(value, type);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToString<T>(value);
        }
    }
}