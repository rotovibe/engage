using System;

namespace Phytel.Services.Serializer
{
    public interface ISerializer
    {
        object Deserialize(string value, Type targetType);

        T Deserialize<T>(string value);

        string Serialize(object value, Type type);

        string Serialize<T>(T value);
    }
}