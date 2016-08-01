using System;
namespace Phytel.API.Common
{
    public interface IHelpers
    {
        string BuildURL(string baseURL, string userId);
        object ConvertToAppropriateType(object p);
        System.Collections.Generic.List<MongoDB.Bson.BsonValue> ConvertToBsonValueList(object p);
        System.Collections.Generic.List<MongoDB.Bson.ObjectId> ConvertToObjectIdList(System.Collections.Generic.List<string> stringList);
        System.Collections.Generic.List<string> ConvertToStringList(System.Collections.Generic.List<MongoDB.Bson.ObjectId> objectIds);
        void LogException(int processId, Exception ex);
        string TrimAndLimit(string value, int limit);
        void SerializeObject<T>(T obj, string filePath);
        object DeserializeObject<T>(string filePath);
    }
}
