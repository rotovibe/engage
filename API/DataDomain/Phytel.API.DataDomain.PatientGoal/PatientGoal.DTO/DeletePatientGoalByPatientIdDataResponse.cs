using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class DeletePatientGoalByPatientIdDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public List<DeletedPatientGoal> DeletedPatientGoals { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class DeletedPatientGoal
    {
        public string Id { get; set; }
        public List<string> PatientBarrierIds { get; set; }
        public List<string> PatientTaskIds { get; set; }
        public List<string> PatientInterventionIds { get; set; }
    }
}
