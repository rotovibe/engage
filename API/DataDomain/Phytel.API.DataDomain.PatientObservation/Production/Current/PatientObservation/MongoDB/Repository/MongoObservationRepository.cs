using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation;
using MB = MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class MongoObservationRepository : IPatientObservationRepository
    {
        private string _dbName = string.Empty;

        static MongoObservationRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ObservationValue)) == false)
                    BsonClassMap.RegisterClassMap<ObservationValue>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEObservation)) == false)
                    BsonClassMap.RegisterClassMap<MEObservation>();
            }
            catch { }
            #endregion
        }

        public MongoObservationRepository(string contractDBName)
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
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object FindByID(string entityID)
        {
            ObservationData odL = null;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEObservation.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);

                    MEObservation o = ctx.Observations.Collection.Find(mQuery).FirstOrDefault();

                    if (o != null)
                    {
                        odL = new ObservationData
                        {
                            CodingSystem = o.CodingSystemId.ToString(),
                            CodingSystemCode = o.Code,
                            DeleteFlag = o.DeleteFlag,
                            Description = o.Description,
                            GroupId = o.GroupId != null ? o.GroupId.ToString() : null,
                            LowValue = o.LowValue,
                            HighValue = o.HighValue,
                            Id = o.Id.ToString(),
                            LastUpdatedOn = o.LastUpdatedOn,
                            ObservationTypeId = o.ObservationTypeId.ToString(),
                            Order = o.Order,
                            Source = o.Source,
                            Standard = o.Standard,
                            Status = (int)o.Status,
                            TTLDate = o.TTLDate,
                            Units = o.Units,
                            UpdatedBy = o.UpdatedBy.ToString(),
                            Version = o.Version,
                            CommonName = o.CommonName
                        };
                    }
                }
                return odL;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        object IRepository.Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        object IRepository.InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        void IRepository.Delete(object entity)
        {
            throw new NotImplementedException();
        }

        void IRepository.DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        Tuple<string, IEnumerable<object>> IRepository.Select(APIExpression expression)
        {
            throw new NotImplementedException();
        }

        IEnumerable<object> IRepository.SelectAll()
        {
            throw new NotImplementedException();
        }

        object IRepository.Update(object entity)
        {
            throw new NotImplementedException();
        }

        void IRepository.CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InitializeProblem(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object GetObservationsByType(object type, bool? standard, bool? status)
        {
            List<ObservationData> odL = new List<ObservationData>();
            try
            {
                if (type != null)
                {
                    using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = BuildQuery(type, standard, status);
                        IMongoQuery mQuery = Query.And(queries);

                        List<MEObservation> meObs = ctx.Observations.Collection.Find(mQuery).ToList();

                        if (meObs != null && meObs.Count > 0)
                        {
                            meObs.ForEach(o =>
                            {
                                odL.Add(new ObservationData
                                {
                                    CodingSystem = o.CodingSystemId.ToString(),
                                    CodingSystemCode = o.Code,
                                    DeleteFlag = o.DeleteFlag,
                                    Description = o.Description,
                                    GroupId = o.GroupId != null ? o.GroupId.ToString() : null,
                                    LowValue = o.LowValue,
                                    HighValue = o.HighValue,
                                    Id = o.Id.ToString(),
                                    LastUpdatedOn = o.LastUpdatedOn,
                                    ObservationTypeId = o.ObservationTypeId.ToString(),
                                    Order = o.Order,
                                    Source = o.Source,
                                    Standard = o.Standard,
                                    Status = (int)o.Status,
                                    TTLDate = o.TTLDate,
                                    Units = o.Units,
                                    UpdatedBy = o.UpdatedBy.ToString(),
                                    Version = o.Version,
                                    CommonName = o.CommonName
                                });
                            });
                        }
                    }
                }
                return odL;
            }
            catch (Exception ex)
            {
                throw new Exception("PatientObservationDD:GetObservationsByType()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<IMongoQuery> BuildQuery(object type, bool? standard, bool? activeStatus)
        {
            List<IMongoQuery> queries = new List<IMongoQuery>();
            queries.Add(Query.EQ(MEObservation.ObservationTypeProperty, ObjectId.Parse(type as string)));
            if (standard != null)
            {
                    queries.Add(Query.EQ(MEObservation.StandardProperty, standard));
            }
            if (activeStatus != null)
            {
                if ((bool)activeStatus)
                    queries.Add(Query.EQ(MEObservation.StatusProperty, 1)); // active
            }
            queries.Add(Query.EQ(MEObservation.DeleteFlagProperty, false));
            return queries;
        }

        public IEnumerable<object> FindObservationIdByPatientId(string Id)
        {
            throw new NotImplementedException();
        }
        
        public object FindRecentObservationValue(string observationTypeId, string patientId)
        {
            throw new NotImplementedException();
        }

        public List<ObservationStateData> GetAllowedObservationStates()
        {
            List<ObservationStateData> states = new List<ObservationStateData>();
            ObservationState[] values = (ObservationState[])Enum.GetValues(typeof(ObservationState));
            foreach (var item in values)
            {
                ObservationStateData state = null;
                switch (item)
                {
                    
                    case ObservationState.Complete:
                        state = new ObservationStateData { Id = (int)ObservationState.Complete, Name = ObservationState.Complete.ToString(), TypeIds = new List<string> { Constants.LabLookUpId, Constants.VitalsLookUpId } };
                        break;
                    case ObservationState.Active:
                        state = new ObservationStateData { Id = (int)ObservationState.Active, Name = ObservationState.Active.ToString(), TypeIds = new List<string> { Constants.ProblemLookUpId } };
                        break;
                    case ObservationState.Inactive:
                        state = new ObservationStateData { Id = (int)ObservationState.Inactive, Name = ObservationState.Inactive.ToString(), TypeIds = new List<string> { Constants.ProblemLookUpId } };
                        break;
                    case ObservationState.Resolved:
                        state = new ObservationStateData { Id = (int)ObservationState.Resolved, Name = ObservationState.Resolved.ToString(), TypeIds = new List<string> { Constants.ProblemLookUpId } };
                        break;
                    case ObservationState.Decline:
                        state = new ObservationStateData { Id = (int)ObservationState.Decline, Name = ObservationState.Decline.ToString(), TypeIds = new List<string> { Constants.LabLookUpId, Constants.VitalsLookUpId } };
                        break;  
                }
                states.Add(state);
            }
            return states;
        }

        public IEnumerable<object> GetActiveObservations()
        {
            List<ObservationData> odL = null;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEObservation.StatusProperty, Status.Active));
                    queries.Add(Query.EQ(MEObservation.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);

                    List<MEObservation> meObs = ctx.Observations.Collection.Find(mQuery).ToList();

                    if (meObs != null && meObs.Count > 0)
                    {
                        odL = new List<ObservationData>();
                        meObs.ForEach(o =>
                        {
                            odL.Add(new ObservationData
                            {
                                CodingSystem = o.CodingSystemId.ToString(),
                                CodingSystemCode = o.Code,
                                DeleteFlag = o.DeleteFlag,
                                Description = o.Description,
                                GroupId = o.GroupId != null ? o.GroupId.ToString() : null,
                                LowValue = o.LowValue,
                                HighValue = o.HighValue,
                                Id = o.Id.ToString(),
                                LastUpdatedOn = o.LastUpdatedOn,
                                ObservationTypeId = o.ObservationTypeId.ToString(),
                                Order = o.Order,
                                Source = o.Source,
                                Standard = o.Standard,
                                Status = (int)o.Status,
                                TTLDate = o.TTLDate,
                                Units = o.Units,
                                UpdatedBy = o.UpdatedBy.ToString(),
                                Version = o.Version,
                                CommonName = o.CommonName
                            });
                        });
                    }
                }
                return odL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UserId { get; set; }


        public object FindByObservationID(string entityId, string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }


        public object FindByID(string entityID, bool includeDeletedObservations)
        {
            throw new NotImplementedException();
        }
    }
}
