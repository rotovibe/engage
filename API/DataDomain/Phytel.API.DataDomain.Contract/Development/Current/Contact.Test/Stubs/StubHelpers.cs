using Phytel.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contract.Test.Stubs
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

        public List<MongoDB.Bson.BsonValue> ConvertToBsonValueList(object p)
        {
            throw new NotImplementedException();
        }

        public List<MongoDB.Bson.ObjectId> ConvertToObjectIdList(List<string> stringList)
        {
            throw new NotImplementedException();
        }

        public List<string> ConvertToStringList(List<MongoDB.Bson.ObjectId> objectIds)
        {
            throw new NotImplementedException();
        }

        public void LogException(int processId, Exception ex)
        {
            
        }
    }
}
