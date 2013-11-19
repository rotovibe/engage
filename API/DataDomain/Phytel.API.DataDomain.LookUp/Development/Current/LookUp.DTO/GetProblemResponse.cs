using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
   public class GetProblemResponse
   {
        public Problem Problem { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Problem
    {
        public string ProblemID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }
    }
}
