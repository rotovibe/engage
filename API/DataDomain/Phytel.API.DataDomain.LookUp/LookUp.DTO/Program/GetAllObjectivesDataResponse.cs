using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllObjectivesDataResponse : IDomainResponse
    {
        public List<ObjectiveData> ObjectivesData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ObjectiveData : IdNamePair
    {
        public List<IdNamePair> CategoriesData { get; set; }
    }
}
