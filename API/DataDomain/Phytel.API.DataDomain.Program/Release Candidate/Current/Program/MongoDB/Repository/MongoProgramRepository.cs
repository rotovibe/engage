using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.Program
{
    public class MongoProgramRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        static MongoProgramRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ProgramBase)) == false)
                    BsonClassMap.RegisterClassMap<ProgramBase>();
            }
            catch { }
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEProgram)) == false)
                    BsonClassMap.RegisterClassMap<MEProgram>();
            }
            catch { }
            #endregion
        }

        public MongoProgramRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

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
            //DTO.Program program = null;
            //using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            //{
            //    program = (from p in ctx.Programs
            //               where p.Id == ObjectId.Parse(entityID)
            //               select new DTO.Program
            //               {
            //                   ProgramID = p.Id.ToString(),
            //                   TemplateName = p.TemplateName,
            //                   Name = p.Name,
            //                   AuthoredBy = p.AuthoredBy,
            //                   Client = p.Client,
            //                   Description = p.Description,
            //                   EndDate = p.EndDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EndDate),
            //                   StartDate = p.StartDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.StartDate),
            //                   ObjectivesInfo = p.ObjectivesInfo.Select(x => new DTO.ObjectivesDetail
            //                   {
            //                        Id = x.Id.ToString(),
            //                       Unit = x.Unit,
            //                       Status = (int)x.Status,
            //                       Value = x.Value
            //                   }).ToList(),
            //                   Locked = p.Locked,
            //                   EligibilityRequirements = p.EligibilityRequirements,
            //                   EligibilityEndDate = p.EligibilityEndDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EligibilityEndDate),
            //                   EligibilityStartDate = p.EligibilityStartDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EligibilityStartDate),
            //                   ShortName = p.ShortName,
            //                   Status = p.Status,
            //                   Version = p.Version
            //               }).FirstOrDefault();
            //}
            //return program;
            throw new NotImplementedException();
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
        
        public MEProgram FindByID(string entityID, bool temp)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
    }
}
