﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;

namespace Phytel.API.DataDomain.Medication.Test
{
    class StubMedicationRepository<TContext> : IMongoMedicationRepository where TContext : MedicationMongoContext
    {
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public StubMedicationRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public StubMedicationRepository(TContext context)
        {
            Context = context;
        }
        
        public object FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }

        public object FindNDCCodes(object request)
        {
            List<string> result = new List<string>();
            result.Add("64193-225");
            result.Add("64193-224");
            return result;
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
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

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public object Search(object request)
        {
            throw new NotImplementedException();
        }


        public object FindByName(object request)
        {
            throw new NotImplementedException();
        }

        public object Find(object request)
        {
            throw new NotImplementedException();
        }
    }
}