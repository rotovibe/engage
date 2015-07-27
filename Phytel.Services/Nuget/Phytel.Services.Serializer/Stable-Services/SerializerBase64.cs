using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Phytel.Services.Serializer
{
    public class SerializerBase64 : ISerializer
    {
        public object Deserialize(string value, Type targetType)
        {
            byte[] itemBytes = Convert.FromBase64String(value);

            MemoryStream stream = new MemoryStream();
            stream.Write(itemBytes, 0, itemBytes.Length);
            stream.Position = 0;

            BinaryFormatter formatter = new BinaryFormatter();
            object result = formatter.Deserialize(stream);

            return result;
        }

        public T Deserialize<T>(string value)
        {
            return (T)Deserialize(value, typeof(T));
        }

        public string Serialize(object value, Type type)
        {
            string rvalue = string.Empty;

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, value);

            rvalue = Convert.ToBase64String(stream.ToArray());

            return rvalue;
        }

        public string Serialize<T>(T value)
        {
            return Serialize(value, typeof(T));
        }
    }
}