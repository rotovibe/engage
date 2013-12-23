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

        public object Insert(T newEntity)
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
            DTO.Program program = null;
            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                program = (from p in ctx.Programs
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.Program
                           {
                               ProgramID = p.Id.ToString(),
                               TemplateName = p.TemplateName,
                               Name = p.Name,
                               AuthoredBy = p.AuthoredBy,
                               Client = p.Client,
                               Description = p.Description,
                               EndDate = p.EndDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EndDate),
                               StartDate = p.StartDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.StartDate),
                               ObjectivesInfo = p.ObjectivesInfo.Select(x => new DTO.ObjectivesInfo
                               {
                                   ID = x.ID,
                                   Measurement = x.Measurement,
                                   Status = x.Status,
                                   Value = x.Value
                               }).ToList(),
                               Locked = p.Locked,
                               EligibilityRequirements = p.EligibilityRequirements,
                               EligibilityEndDate = p.EligibilityEndDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EligibilityEndDate),
                               EligibilityStartDate = p.EligibilityStartDate == null ? string.Empty : String.Format("{0:MM/dd/yyyy}", p.EligibilityStartDate),
                               ProgramStatus = p.ProgramStatus,
                               ShortName = p.ShortName,
                               Status = p.Status,
                               Version = p.Version
                           }).FirstOrDefault();
            }
            return program;
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            List<ProgramInfo> result = new List<ProgramInfo>();

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                IMongoQuery mQuery = Query.EQ(MEContractProgram.StatusProperty, 1);
                result = ctx.ContractPrograms.Collection.Find(mQuery).Select(r => new ProgramInfo
                {
                    Name = r.Name,
                    ProgramID = r.Id.ToString(),
                    ProgramStatus = r.ProgramStatus,
                    ShortName = r.ShortName,
                    Status = (int)r.Status
                }).ToList();
            }
            return result;
        }

        public Outcome InsertPatientToProgramAssignment(PutProgramToPatientRequest request)
        {
            try
            {
                Outcome result = new Outcome();
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findQ = Query.And(
                        Query.In(MEPatientProgram.ProgramStateProperty, new List<BsonValue> { BsonValue.Create(0), BsonValue.Create(1) }),
                        Query<MEPatientProgram>.EQ(b => b.PatientId, ObjectId.Parse(request.PatientId)),
                        Query<MEPatientProgram>.EQ(b => b.ContractProgramId, ObjectId.Parse(request.ContractProgramId)));

                    List<MEPatientProgram> pp = ctx.PatientPrograms.Collection.Find(findQ).ToList();

                    if (pp.Count == 0)
                    {
                        var findcp = Query<MEContractProgram>.EQ(b => b.Id, ObjectId.Parse(request.ContractProgramId));
                        MEContractProgram cp = ctx.ContractPrograms.Collection.Find(findcp).FirstOrDefault();

                        ctx.PatientPrograms.Collection.Insert(new MEPatientProgram
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            AuthoredBy = cp.AuthoredBy,
                            Client = cp.Client,
                            ProgramState = Common.ProgramState.NotStarted,
                            ProgramStatus = "0",
                            ContractProgramId = cp.Id,
                            DeleteFlag = cp.DeleteFlag,
                            Description = cp.Description,
                            EligibilityEndDate = cp.EligibilityEndDate,
                            EligibilityRequirements = cp.EligibilityRequirements,
                            EligibilityStartDate = cp.EligibilityStartDate,
                            EndDate = cp.EndDate,
                            LastUpdatedOn = cp.LastUpdatedOn,
                            Locked = cp.Locked,
                            Modules = cp.Modules,
                            Name = cp.Name,
                            ObjectivesInfo = cp.ObjectivesInfo,
                            ShortName = cp.ShortName,
                            StartDate = cp.StartDate,
                            Status = cp.Status

                        });

                        result.Result = 1;
                        result.Reason = "Successfully assigned this program for the patient";
                    }
                    else
                    {
                        result.Result = 0;
                        result.Reason = pp[0].Name + " is already assigned";
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
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
