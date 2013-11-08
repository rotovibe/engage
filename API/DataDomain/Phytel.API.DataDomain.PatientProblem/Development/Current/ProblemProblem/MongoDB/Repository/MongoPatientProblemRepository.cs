using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.AppDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;

namespace Phytel.API.DataDomain.PatientProblem
{
    public class MongoPatientProblemRepository<T> : IPatientProblemRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProblemRepository(string contractDBName)
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
            throw new NotImplementedException();
        }

        public Tuple<int, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            IQueryable<T> patients = null;
            Tuple<int, IQueryable<T>> response = new Tuple<int, IQueryable<T>>(0, patients);
            //IQueryable<MEPatientProblem> patientProblems = null;
            //using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
            //{
            //    patientProblems = ctx.PatientProblems.Collection.(ObjectId.Parse(entityID));
            //}
            return response;
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
