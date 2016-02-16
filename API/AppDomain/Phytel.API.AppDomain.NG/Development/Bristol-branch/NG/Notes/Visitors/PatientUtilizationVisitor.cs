using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Utilization;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class PatientUtilizationVisitor : VisitorBase
    {
        public override List<PatientNote> Visit(ref List<PatientNote> result)
        {
            // get aggregate data for utilization note types.
            var utilManager = new UtilizationManager {};
            var response = utilManager.GetPatientUtilizations(new GetPatientUtilizationsRequest
            {
                ContractNumber = ContractNumber,
                PatientId = PatientId,
                UserId = UserId,
                Version = Version//,
                //Count = Count
            });

            if (response.Utilizations != null)
                result.AddRange(response.Utilizations.Select(Mapper.Map<PatientNote>));

            return result;
        }
    }
}
