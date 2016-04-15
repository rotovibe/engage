using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.ServiceInterface.ServiceModel;

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

        public SaveCareTeamDataResponse InsertCareTeam(SaveCareTeamDataRequest request)
        {
            var response = new SaveCareTeamDataResponse();

            if(request == null)
                throw new ArgumentNullException("request");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if(repo == null)
                throw new Exception("Repository is null");

           // var members = request.CareTeamData.Members;

            //if (!members.IsNullOrEmpty())
            //{
            //    var memberGroups = members.GroupBy(g => g.ContactId).Select(grp => new { ContactId = grp.Key , Count = grp.Count()});

            //    if(memberGroups.Any(m =>  m.Count > 1))
            //        throw new Exception("Care Team cannot have multiple members with same ContactId");

            //}

            repo.Insert(request);

            return response;
        }

        public UpdateCareTeamMemberDataResponse UpdateCareTeamMember(UpdateCareTeamMemberDataRequest request)
        {
            var response = new UpdateCareTeamMemberDataResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if (repo == null)
                throw new Exception("Repository is null");

            try
            {
                if (repo.UpdateCareTeamMember(request)) response.Status = new ResponseStatus();
            }
            catch (ApplicationException ex)
            {
                response.Status = new ResponseStatus(HttpStatusCode.NotFound.ToString(), ex.Message);
            }
            catch (Exception)
            {                    
                throw;
            }                      
            return response;
        }
        public GetCareTeamDataResponse GetCareTeam(GetCareTeamDataRequest request)
        {
            var response = new GetCareTeamDataResponse();

            if(request == null)
                throw new ArgumentNullException("request");
            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("ContactId");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);
            if(repo == null)
                throw new Exception("Repository is null");

           response.CareTeamData = (CareTeamData)repo.GetCareTeamByContactId(request.ContactId);
           return response;
        }

        public bool DeleteCareTeamMember(DeleteCareTeamMemberDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
