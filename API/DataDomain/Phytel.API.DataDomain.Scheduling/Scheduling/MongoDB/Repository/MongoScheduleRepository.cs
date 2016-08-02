using MongoDB.Driver;
using Phytel.API.Common.Data;
using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Scheduling.DTO;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using MB = MongoDB.Driver.Builders;
using MongoDB.Driver.Builders;
using System.Linq;

namespace Phytel.API.DataDomain.Scheduling
{
    public class MongoScheduleRepository : ISchedulingRepository
    {
        private string _dbName = string.Empty;

        public MongoScheduleRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public string UserId { get; set; }

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
            ScheduleData scheduleData = null;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MESchedule.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MESchedule.IdProperty, ObjectId.Parse(entityID)));
                    var mQuery = Query.And(queries);
                    MESchedule meSch = ctx.Schedules.Collection.Find(mQuery).FirstOrDefault();
                    if (meSch != null)
                    {
                        scheduleData = new ScheduleData
                        {
                            AssignedToId = meSch.AssignedToId.ToString() ?? null,
                            CategoryId = meSch.Category.ToString() ?? null,
                            ClosedDate = meSch.ClosedDate,
                            CreatedById = meSch.RecordCreatedBy.ToString() ?? null,
                            CreatedOn = meSch.RecordCreatedOn,
                            Description = meSch.Description,
                            DueDate = meSch.DueDate,
                            StartTime = meSch.StartTime,
                            Duration = meSch.Duration,
                            DueDateRange = meSch.DueDateRange,
                            Id = meSch.Id.ToString(),
                            PatientId = meSch.PatientId.ToString() ?? null,
                            PriorityId = (int)meSch.Priority,
                            ProgramIds = Helper.ConvertToStringList(meSch.ProgramIds),
                            StatusId = (int)meSch.Status,
                            Title = meSch.Title,
                            UpdatedOn = meSch.LastUpdatedOn,
                            TypeId = (int)meSch.Type,
                            DefaultAssignment = meSch.DefaultAssignment
                        };
                    }
                }
                return scheduleData;
            }
            catch (Exception) { throw; }
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

        public GetToDosDataResponse FindToDos(object request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindToDosWithAProgramId(string entityId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            throw new NotImplementedException();
        }


        public object FindByID(string entityID, bool includeDeletedToDo)
        {
            throw new NotImplementedException();
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Select(List<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}
