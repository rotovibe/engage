using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.Common.Web;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Text;

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

            var id = repo.Insert(request) as string;

            response.Id = id;


            return response;
        }

        public UpdateCareTeamMemberDataResponse UpdateCareTeamMember(UpdateCareTeamMemberDataRequest request)
        {
            var response = new UpdateCareTeamMemberDataResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            if (request.CareTeamMemberData == null)
                throw new ArgumentNullException("CareTeamMemberData is Null","request");

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("Null or Empty ContactId", "request");

            if (string.IsNullOrEmpty(request.CareTeamId))
                throw new ArgumentNullException("Null or empty CareTeamId", "request");

            if (string.IsNullOrEmpty(request.Id))
                throw new ArgumentNullException("Null or empty MemberId", "request");

            if (request.Id != request.CareTeamMemberData.Id)
                throw new ArgumentNullException("CareTeamMemberData.Id and Id are different","request");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if (repo == null)
                throw new Exception("Repository is null");
            
            try
            {
                if (!repo.CareTeamMemberExist(request.CareTeamId, request.Id))                
                    throw new Exception(string.Format("Care Team Member {0} does not exist", request.Id));
                    
                repo.UpdateCareTeamMember(request);
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

        public void DeleteCareTeamMember(DeleteCareTeamMemberDataRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            if(string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentException("Empty ContactId", "request");

            if (string.IsNullOrEmpty(request.CareTeamId))
                throw new ArgumentException("Null or empty CareTeamId", "request");

            if (string.IsNullOrEmpty(request.MemberId))
                throw new ArgumentException("Null or empty MemberId", "request");

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);
            if (repo == null)
                throw new Exception("Repository is null");

            repo.DeleteCareTeamMember(request);
        }

        public DeleteCareTeamDataResponse DeleteCareTeam(DeleteCareTeamDataRequest request)
        {
            var response = new DeleteCareTeamDataResponse();

            if (request == null)
                throw new ArgumentNullException("request");           

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("Null or Empty ContactId", "request");

            if (string.IsNullOrEmpty(request.Id))
                throw new ArgumentNullException("Null or empty Care Team Id", "request");                        

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if (repo == null)
                throw new Exception("Repository is null");

            try
            {
                if (!repo.CareTeamExist(request.Id))
                    throw new Exception(string.Format("Care Team {0} does not exist", request.Id));

                repo.Delete(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public UndoDeleteCareTeamDataResponse UndoDeleteCareTeam(UndoDeleteCareTeamDataRequest request)
        {
            var response = new UndoDeleteCareTeamDataResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("Null or Empty ContactId", "request");

            if (string.IsNullOrEmpty(request.Id))
                throw new ArgumentNullException("Null or empty Care Team Id", "request");
            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if (repo == null)
                throw new Exception("Repository is null");

            try
            {
                
                repo.UndoDelete(request);
            }
            catch (Exception)
            {
                throw;
            }
            return response;

        }


        public AddCareTeamMemberDataResponse AddCareTeamMember(AddCareTeamMemberDataRequest request)
        {
            var response = new AddCareTeamMemberDataResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            if (request.CareTeamMemberData == null)
                throw new ArgumentNullException("CareTeamMemberData is Null", "request");

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("Null or Empty ContactId", "request");

            if (string.IsNullOrEmpty(request.CareTeamId))
                throw new ArgumentNullException("Null or empty CareTeamId", "request");

           

            var repo = _factory.GetCareTeamRepository(request, RepositoryType.CareTeam);

            if (repo == null)
                throw new Exception("Repository is null");

            try
            {
                if (repo.CareTeamMemberContactExist(request.CareTeamId, request.ContactId))
                    throw new Exception(string.Format("Care Team Member {0} already exist", request.ContactId));

               response.Id =  repo.AddCareTeamMember(request);
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }
    }
}
