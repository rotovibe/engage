using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.DataDomain.ProgramDesign.DTO;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoProgramDesignRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoProgramDesignRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public string UserId { get; set; }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
            //try
            //{
            //    MEProgram cp = null;
            //    using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            //    {
            //        var findcp = MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
            //        cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();
            //    }
            //    return cp;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("DD:Program:FindByID()::" + ex.Message, ex.InnerException);
            //}
        }

        public object FindByName(string entityName)
        {
            throw new NotImplementedException();
            //try
            //{
            //    DTO.Program result = null;

            //    using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            //    {
            //        var findcp = MB.Query<MEProgram>.EQ(b => b.Name, entityName);
            //        MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

            //        if (cp != null)
            //        {
            //            result = new DTO.Program
            //            {
            //                ProgramID = cp.Id.ToString()
            //            };
            //        }
            //        else
            //        {
            //            throw new ArgumentException("ProgramName is not valid or is missing from the records.");
            //        }
            //    }
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("DD:MongoProgramDesignRepository:FindByName()::" + ex.Message, ex.InnerException);
            //}
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
