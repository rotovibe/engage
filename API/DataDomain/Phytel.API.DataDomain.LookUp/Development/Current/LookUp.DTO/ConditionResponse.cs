using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
   public class ConditionResponse
   {
        public Condition Condition { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Condition
    {
        public string ConditionID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
