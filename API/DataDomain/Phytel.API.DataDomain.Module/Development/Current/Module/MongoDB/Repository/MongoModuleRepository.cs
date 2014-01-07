using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Module;
using Phytel.API.DataDomain.Module.MongoDB.DTO;

namespace Phytel.API.DataDomain.Module
{
    public class MongoModuleRepository<T> : IModuleRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoModuleRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<T> entities)
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
            DTO.Module module = null;
            using (ModuleMongoContext ctx = new ModuleMongoContext(_dbName))
            {
                module = (from m in ctx.Modules
                          where m.Id == ObjectId.Parse(entityID)
                          select new DTO.Module
                          {
                              Id = m.Id.ToString(),
                              Name = m.Name,
                              Description = m.Description,
                              Objectives = m.Objectives.Select(i => i.ID).ToList(),
                              Status = m.Status,
                              Version = m.Version
                          }).FirstOrDefault();
            }
            return module;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
