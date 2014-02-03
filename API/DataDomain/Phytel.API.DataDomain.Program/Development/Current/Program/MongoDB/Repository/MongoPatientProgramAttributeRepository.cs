using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.Program
{
    public class MongoPatientProgramAttributeRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProgramAttributeRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            bool result = false;
            try
            {
                ProgramAttribute pa = (ProgramAttribute)newEntity;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    MEProgramAttribute mepa = new MEProgramAttribute
                    {
                        Status = (Status)pa.Status,
                        RemovedReason = pa.OverrideReason,
                        Population = pa.Population,
                        PlanElementId = ObjectId.Parse(pa.PlanElementId),
                        OverrideReason = pa.OverrideReason,
                        OptOutReason = pa.OptOutReason,
                        OptOutDate = pa.OptOutDate,
                        OptOut = pa.OptOut,
                        Locked = (Locked)pa.Locked,
                        IneligibleReason = pa.IneligibleReason,
                        Completed = (Completed)pa.Completed,
                        AssignedBy = pa.AssignedBy,
                        AssignedOn = pa.AssignedOn,
                        AuthoredBy = pa.AuthoredBy,
                        CompletedBy = pa.CompletedBy,
                        DateCompleted = pa.DateCompleted,
                        DidNotEnrollReason = pa.DidNotEnrollReason,
                        DisEnrollReason = pa.DisEnrollReason,
                        Eligibility = (EligibilityStatus)pa.Eligibility,
                        EligibilityEndDate = pa.EligibilityEndDate,
                        EligibilityOverride = (EligibilityOverride)pa.EligibilityOverride,
                        EligibilityRequirements = pa.EligibilityRequirements,
                        EligibilityStartDate = pa.EligibilityStartDate,
                        EndDate = pa.EndDate,
                        Enrollment = (EnrollmentStatus)pa.Enrollment,
                        GraduatedFlag = (Graduated)pa.GraduatedFlag,
                        StartDate = pa.StartDate
                    };

                    WriteConcernResult wcr =  ctx.ProgramAttributes.Collection.Insert(mepa);
                    if (wcr.Ok)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:Insert():" + ex.Message, ex.InnerException);
            }
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
                GetProgramDetailsSummaryResponse result = new GetProgramDetailsSummaryResponse();

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result.Program = new ProgramDetail
                        {
                            Id = cp.Id.ToString(),
                            Client = cp.Client,
                            ContractProgramId = cp.ContractProgramId.ToString(),
                            Description = cp.Description,
                            Name = cp.Name,
                            PatientId = cp.PatientId.ToString(),
                            ProgramState = (int)cp.ProgramState,
                            ShortName = cp.ShortName,
                            StartDate = cp.StartDate,
                            Status = (int)cp.Status,
                            Version = cp.Version,
                            Eligibility = (int)cp.Eligibility,
                            EligibilityEndDate = cp.EligibilityEndDate,
                            EligibilityRequirements = cp.EligibilityRequirements,
                            EligibilityStartDate = cp.EligibilityStartDate,
                            DidNotEnrollReason = cp.DidNotEnrollReason,
                            DisEnrollReason = cp.DisEnrollReason,
                            EligibilityOverride = (int)cp.EligibilityOverride,
                            Enrollment = (int)cp.Enrollment,
                            GraduatedFlag = cp.GraduatedFlag,
                            IneligibleReason = cp.IneligibleReason,
                            OptOut = cp.OptOut,
                            OptOutDate = cp.OptOutDate,
                            OptOutReason = cp.OptOutReason,
                            OverrideReason = cp.OverrideReason,
                            RemovedReason = cp.RemovedReason,
                            EndDate = cp.EndDate,
                            Completed = cp.Completed,
                            Enabled = cp.Enabled,
                            Next = cp.Next,
                            Order = cp.Order,
                            Previous = cp.Previous,
                            SourceId = cp.SourceId,
                            AssignBy = cp.AssignedBy,
                            AssignDate = cp.AssignedOn,
                            ElementState = (int)cp.State,
                            CompletedBy = cp.CompletedBy,
                            DateCompleted = cp.DateCompleted,
                            ObjectivesInfo = DTOUtils.GetObjectives(cp.ObjectivesInfo),
                            SpawnElement = DTOUtils.GetSpawnElement(cp),
                            Modules = DTOUtils.GetModules(cp.Modules, _dbName)
                        };
                    }
                    else
                    {
                        throw new ArgumentException("ProgramID is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:FindById():" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<object> pAtts = new List<object>();

            mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                // check to see which properties from planelement we need.
                List<MEProgramAttribute> pa = ctx.ProgramAttributes.Collection.Find(mQuery).ToList();

                if (pa != null)
                {
                    pa.ForEach(cp => pAtts.Add(new ProgramAttribute
                    {
                        Id = cp.Id.ToString(),
                        AuthoredBy = cp.AuthoredBy,
                        DidNotEnrollReason = cp.DidNotEnrollReason,
                        DisEnrollReason = cp.DisEnrollReason,
                        Eligibility = (int)cp.Eligibility,
                        EligibilityEndDate = cp.EligibilityEndDate,
                        EligibilityOverride = (int)cp.EligibilityOverride,
                        EligibilityRequirements = cp.EligibilityRequirements,
                        EligibilityStartDate = cp.EligibilityStartDate,
                        EndDate = cp.EndDate,
                        Enrollment = (int)cp.Enrollment,
                        GraduatedFlag = (int)cp.GraduatedFlag,
                        IneligibleReason = cp.IneligibleReason,
                        Locked = (int)cp.Locked,
                        OptOut = cp.OptOut,
                        OptOutDate = cp.OptOutDate,
                        OptOutReason = cp.OptOutReason,
                        OverrideReason = cp.OverrideReason,
                        PlanElementId = cp.PlanElementId.ToString(),
                        Population = cp.Population,
                        RemovedReason = cp.RemovedReason,
                        Status = (int)cp.Status
                    }));
                }
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, pAtts);
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            ProgramAttribute mepa = (ProgramAttribute)entity;
            bool result = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEProgramAttribute>.EQ(b => b.PlanElementId, ObjectId.Parse(mepa.PlanElementId));

                    var uv = new List<MB.UpdateBuilder>();
                    if (mepa.AssignedBy != null) uv.Add(MB.Update.Set(MEProgramAttribute.AssignByProperty, mepa.AssignedBy));
                    if (mepa.AssignedOn != null) uv.Add(MB.Update.Set(MEProgramAttribute.AssignDateProperty, mepa.AssignedOn));
                    if (mepa.AuthoredBy != null) uv.Add(MB.Update.Set(MEProgramAttribute.AuthoredByProperty, mepa.AuthoredBy));
                    if (mepa.CompletedBy != null) uv.Add(MB.Update.Set(MEProgramAttribute.CompletedByProperty, mepa.CompletedBy));
                    if (mepa.DateCompleted != null) uv.Add(MB.Update.Set(MEProgramAttribute.CompletedOnProperty, mepa.DateCompleted));
                    if (mepa.DidNotEnrollReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.DidNotEnrollReasonProperty, mepa.DidNotEnrollReason));
                    if (mepa.DisEnrollReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.DisEnrollReasonProperty, mepa.DisEnrollReason));
                    if (mepa.EligibilityEndDate != null) uv.Add(MB.Update.Set(MEProgramAttribute.EligibilityEndDateProperty, mepa.EligibilityEndDate));
                    if (mepa.EligibilityRequirements != null) uv.Add(MB.Update.Set(MEProgramAttribute.EligibilityRequirementsProperty, mepa.EligibilityRequirements));
                    if (mepa.EligibilityStartDate != null) uv.Add(MB.Update.Set(MEProgramAttribute.EligibilityStartDateProperty, mepa.EligibilityStartDate));
                    if (mepa.EndDate != null) uv.Add(MB.Update.Set(MEProgramAttribute.EndDateProperty, mepa.EndDate));
                    if (mepa.IneligibleReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.IneligibleReasonProperty, mepa.IneligibleReason));
                    if (mepa.OptOutDate != null) uv.Add(MB.Update.Set(MEProgramAttribute.OptOutDateProperty, mepa.OptOutDate));
                    if (mepa.OptOut != null) uv.Add(MB.Update.Set(MEProgramAttribute.OptOutProperty, mepa.OptOut));
                    if (mepa.OptOutReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.OptOutReasonProperty, mepa.OptOutReason));
                    if (mepa.OverrideReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.OverrideReasonProperty, mepa.OverrideReason));
                    if (mepa.Population != null) uv.Add(MB.Update.Set(MEProgramAttribute.PopulationProperty, mepa.Population));
                    if (mepa.RemovedReason != null) uv.Add(MB.Update.Set(MEProgramAttribute.RemovedReasonProperty, mepa.RemovedReason));
                    if (mepa.StartDate != null) uv.Add(MB.Update.Set(MEProgramAttribute.StartDateProperty, mepa.StartDate));
                    if (mepa.Status != 0) uv.Add(MB.Update.Set(MEProgramAttribute.StatusProperty, (Status)mepa.Status));
                    if (mepa.Enrollment != 0) uv.Add(MB.Update.Set(MEProgramAttribute.EnrollmentProperty, (EnrollmentStatus)mepa.Enrollment));
                    if (mepa.EligibilityOverride != 0) uv.Add(MB.Update.Set(MEProgramAttribute.EligibilityOverrideProperty, (EligibilityOverride)mepa.EligibilityOverride));
                    if (mepa.Eligibility != 0) uv.Add(MB.Update.Set(MEProgramAttribute.EligibilityProperty, (EligibilityStatus)mepa.Eligibility));
                    if (mepa.Completed != 0) uv.Add(MB.Update.Set(MEProgramAttribute.CompletedProperty, mepa.Completed));
                    if (mepa.GraduatedFlag != 0) uv.Add(MB.Update.Set(MEProgramAttribute.GraduatedFlagProperty, mepa.GraduatedFlag));
                    if (mepa.Locked != 0) uv.Add(MB.Update.Set(MEProgramAttribute.LockedProperty, mepa.Locked));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult wcr = ctx.ProgramAttributes.Collection.Update(q, update);
                    if (wcr.Ok)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:Update():" + ex.Message, ex.InnerException);
            }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
