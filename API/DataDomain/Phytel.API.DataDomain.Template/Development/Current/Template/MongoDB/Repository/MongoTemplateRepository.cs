using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Template;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.Template
{
    public class MongoTemplateRepository<T> : ITemplateRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoTemplateRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByID(string entityID)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                IMongoQuery mQuery = null;
                List<object> TemplateItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (TemplateMongoContext ctx = new TemplateMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, TemplateItems);
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
