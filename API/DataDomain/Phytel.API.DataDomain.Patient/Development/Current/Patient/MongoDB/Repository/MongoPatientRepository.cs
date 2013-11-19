using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Patient;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientRepository<T> : IPatientRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientRepository(string contractDBName)
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
            PatientResponse patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityID)
                           select new PatientResponse
                            {
                                DOB = p.DOB,
                                FirstName = p.FirstName,
                                Gender = p.Gender,
                                LastName = p.LastName,
                                PatientID = p.Id.ToString(),
                                PreferredName = p.PreferredName,
                                MiddleName = p.MiddleName,
                                Suffix = p.Suffix
                            }).FirstOrDefault();
            }
            return patient;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
            //List<IMongoQuery> queries = new List<IMongoQuery>();

            //queries.Add(Query.EQ(MEPatient.FirstNameProperty, "Greg"));
            //queries.Add(Query.EQ(MEPatient.LastNameProperty, "Tony"));

            //IMongoQuery query2 = Query.And(queries);

            //IMongoQuery query = Query.Or(
            //    Query.EQ(MEPatient.FirstNameProperty, "Greg"),
            //    Query.EQ(MEPatient.LastNameProperty, "Tony"));
        }

        public PatientDetailsResponse Select(string[] patientIds)
        {
            BsonValue[] bsv = new BsonValue[patientIds.Length];
            for (int i = 0; i < patientIds.Length; i++)
            {
                bsv[i] = ObjectId.Parse(patientIds[i]);
            }

            IMongoQuery query = Query.In("_id", bsv);
            List<MEPatient> pr = null;
            List<PatientResponse> pResp = new List<PatientResponse>();
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                pr = ctx.Patients.Collection.Find(query).ToList();
                // convert to a PatientDetailsResponse
                foreach (MEPatient mp in pr)
                {
                    pResp.Add(new PatientResponse
                    {
                        PreferredName = mp.PreferredName,
                        DOB = mp.DOB,
                        FirstName = mp.FirstName,
                        Gender = mp.Gender,
                        LastName = mp.LastName,
                        MiddleName = mp.MiddleName,
                        PatientID = mp.DisplayPatientSystemID,
                        Suffix = mp.Suffix,
                        Version = mp.Version
                    });
                }
            }

            PatientDetailsResponse pdResponse = new PatientDetailsResponse();
            pdResponse.Patients = pResp;

            return pdResponse;
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
