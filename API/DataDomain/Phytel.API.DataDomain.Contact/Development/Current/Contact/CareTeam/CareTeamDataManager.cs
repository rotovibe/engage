using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public class CareTeamDataManager: ICareTeamManager
    {

         private readonly ICareTeamRepositoryFactory _factory;
         public CareTeamDataManager(ICareTeamRepositoryFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            _factory = factory;
        }

        public InsertCareTeamDataResponse InsertCareTeam(InsertCareTeamDataRequest request)
        {
            var response = new InsertCareTeamDataResponse();

            if(request == null)
                throw new ArgumentNullException("request");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if(repo == null)
                throw new Exception("Repository is null");

            repo.Insert(request.CareTeamData);


            return response;
        }
    }
}
