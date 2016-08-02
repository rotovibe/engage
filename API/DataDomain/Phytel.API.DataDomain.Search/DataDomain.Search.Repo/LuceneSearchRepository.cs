
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DataDomain.Search.Repo.LuceneStrategy;
using DTO = Phytel.API.DataDomain.Search.DTO;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo
{
    public class LuceneSearchRepository : IMongoSearchRepository 
    {
        public string ContractDBName { get; set; }
        public string UserId { get; set; }
        public IMedNameLuceneStrategy<MedNameSearchDocData, TextValuePair> MedNameStrategy { get; set; }

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
            try
            {
                object result = null;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
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


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public List<TextValuePair> Search(object entity)
        {
            //var matches = MedNameStrategy.Search(entity as string);
            List<TextValuePair> list = MedNameStrategy.Search(entity as string); //new List<TextValuePair> { new TextValuePair{Text = "Test1", Value="test1"} };
            return list;
        }
    }
}
