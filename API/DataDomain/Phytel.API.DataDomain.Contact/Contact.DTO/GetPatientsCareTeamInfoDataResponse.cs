using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetPatientsCareTeamInfoDataResponse : IDomainResponse
    {
        public List<PatientCareTeamInfo>  ContactCareTeams { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
   
