using System.Collections.Generic;
using ServiceStack;
using DataDomain.LookUp.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllProblemsDataResponse : IDomainResponse
   {
       public List<ProblemData> Problems { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}
