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
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientRepository<T> : IPatientRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(T newEntity)
        {
            PutPatientDataRequest request = newEntity as PutPatientDataRequest;
            MEPatient patient = new MEPatient
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Suffix = request.Suffix,
                PreferredName = request.PreferredName,
                Gender = request.Gender,
                DOB = request.DOB,
                Version = request.Version,
                //UpdatedBy = security token user id,
                //DisplayPatientSystemID
                TTLDate = null,
                DeleteFlag = false,
                LastUpdatedOn = System.DateTime.Now
            };

            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                ctx.Patients.Collection.Insert(patient);
            }

            return new PutPatientDataResponse
            {
                Id = patient.Id.ToString()
            };


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
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.PatientData
                            {
                                ID = p.Id.ToString(),
                                DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                                FirstName = p.FirstName,
                                Gender = p.Gender,
                                LastName = p.LastName,
                                PreferredName = p.PreferredName,
                                MiddleName = p.MiddleName,
                                Suffix = p.Suffix,
                                Priority = (DTO.Priority)((int)p.Priority),
                                DisplayPatientSystemID = p.DisplayPatientSystemID.ToString()
                            }).FirstOrDefault();
            }
            return patient;
        }

        public object FindByID(string entityId, string userId)
        {
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityId)
                           select new DTO.PatientData
                           {
                               ID = p.Id.ToString(),
                               DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                               FirstName = p.FirstName,
                               Gender = p.Gender,
                               LastName = p.LastName,
                               PreferredName = p.PreferredName,
                               MiddleName = p.MiddleName,
                               Suffix = p.Suffix,
                               Priority = (DTO.Priority)((int)p.Priority),
                               DisplayPatientSystemID = p.DisplayPatientSystemID.ToString(),
                               Flagged = GetFlaggedStatus(entityId, userId)
                           }).FirstOrDefault();
            }
            return patient;
        }

        private bool GetFlaggedStatus(string entityId, string userId)
        {
            bool result = false;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                var patientUsr = FindPatientUser(entityId, userId, ctx);

                if (patientUsr != null)
                {
                    result = patientUsr.Flagged;
                }
            }
            return result;
        }

        private static MEPatientUser FindPatientUser(string entityId, string userId, PatientMongoContext ctx)
        {
            var findQ = Query.And(
                Query<MEPatientUser>.EQ(b => b.PatientId, ObjectId.Parse(entityId)),
                Query<MEPatientUser>.EQ(b => b.UserId, userId)
            );

            var patientUsr = ctx.PatientUsers.Collection.Find(findQ).FirstOrDefault();
            return patientUsr;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
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

        public GetPatientsDataResponse Select(string[] patientIds)
        {
            BsonValue[] bsv = new BsonValue[patientIds.Length];
            for (int i = 0; i < patientIds.Length; i++)
            {
                bsv[i] = ObjectId.Parse(patientIds[i]);
            }

            IMongoQuery query = Query.In("_id", bsv);
            List<MEPatient> pr = null;
            List<DTO.PatientData> pResp = new List<DTO.PatientData>();
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                pr = ctx.Patients.Collection.Find(query).ToList();
                // convert to a PatientDetailsResponse
                foreach (MEPatient mp in pr)
                {
                    pResp.Add(new DTO.PatientData
                    {
                        ID = mp.Id.ToString(),
                        PreferredName = mp.PreferredName,
                        DOB = mp.DOB,
                        FirstName = mp.FirstName,
                        Gender = mp.Gender,
                        LastName = mp.LastName,
                        MiddleName = mp.MiddleName,
                        Suffix = mp.Suffix,
                        Version = mp.Version,
                        Priority = (DTO.Priority)((int)mp.Priority),
                        DisplayPatientSystemID = mp.DisplayPatientSystemID.ToString()
                    });
                }
            }

            GetPatientsDataResponse pdResponse = new GetPatientsDataResponse();
            pdResponse.Patients = pResp;

            return pdResponse;
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), SortBy.Null,
                                                MongoDB.Driver.Builders.Update.Set(MEPatient.PriorityProperty, (MEPriority)request.Priority));
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {

                    var patientUsr = FindPatientUser(request.PatientId, request.UserId, ctx);

                    if (patientUsr == null)
                    {
                        ctx.PatientUsers.Collection.Insert(new MEPatientUser
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            UserId = request.UserId,
                            Flagged = Convert.ToBoolean(request.Flagged),
                            Version = "v1",
                            LastUpdatedOn = System.DateTime.UtcNow,
                            DeleteFlag = false
                        });
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                    else
                    {

                        var pUQuery = new QueryDocument(MEPatientUser.PatientIdProperty, ObjectId.Parse(request.PatientId));
                        var sortBy = new SortByBuilder().Ascending("_id");
                        UpdateBuilder updt = new UpdateBuilder().Set("flg", Convert.ToBoolean(request.Flagged));
                        var pt = ctx.PatientUsers.Collection.FindAndModify(pUQuery, sortBy, updt, true);
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEPatientUser.PatientIdProperty, ObjectId.Parse(request.Id));
                    
                    UpdateBuilder updt = new UpdateBuilder()
                        .Set("fn", request.FirstName)
                        .Set("ln", request.LastName)
                        .Set("mn", request.MiddleName)
                        .Set("sfx", request.Suffix)
                        .Set("pfn", request.PreferredName)
                        .Set("gn", request.Gender)
                        .Set("dob", request.DOB)
                        .Set("pri", request.Priority)
                        .Set("v", request.Version);

                    var sortBy = new SortByBuilder().Ascending("_id");
                    var pt = ctx.PatientUsers.Collection.FindAndModify(pUQuery, sortBy, updt, true);
                    
                    response.Id = request.Id;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public object Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
