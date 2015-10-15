using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Contact.Test.Stubs
{
    public class StubHelpers : IHelpers
    {

        public string BuildURL(string baseURL, string userId)
        {
            throw new NotImplementedException();
        }

        public object ConvertToAppropriateType(object p)
        {
            throw new NotImplementedException();
        }

        public List<BsonValue> ConvertToBsonValueList(object p)
        {
            throw new NotImplementedException();
        }

        public List<ObjectId> ConvertToObjectIdList(List<string> stringList)
        {
            throw new NotImplementedException();
        }

        public List<string> ConvertToStringList(List<ObjectId> objectIds)
        {
            throw new NotImplementedException();
        }

        public void LogException(int processId, Exception ex)
        {
            throw new NotImplementedException();
        }

        public string TrimAndLimit(string value, int limit)
        {
            throw new NotImplementedException();
        }

        public void SerializeObject<T>(T obj, string filePath)
        {
            throw new NotImplementedException();
        }

        public object DeserializeObject<T>(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
