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
using Phytel.API.AppDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program
{
    public class MongoProgramRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoProgramRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public T Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public T InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            DTO.Program program = null;
            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                program = (from p in ctx.Programs
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.Program
                           {
                               ProgramID = p.Id.ToString(),
                               Name = p.Name,
                               AuthoredBy = p.AuthoredBy,
                               Client = p.Client,
                               Description = p.Description,
                               EndDate = p.EndDate.ToString(),
                               StartDate = p.StartDate.ToString(),
                               ObjectivesInfo = p.ObjectivesInfo.Select(x => new DTO.ObjectivesInfo
                               {
                                   ID = x.ID,
                                   Measurement = x.Measurement,
                                   Status = x.Status,
                                   Value = x.Value
                               }).ToList(),
                               Locked = p.Locked,
                               ProgramStatus = p.ProgramStatus,
                               ShortName = p.ShortName,
                               Status = p.Status,
                               Version = p.Version
                           }).FirstOrDefault();
            }
            return program;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
