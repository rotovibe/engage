using System;
using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG
{
    public class ContactManager : ManagerBase, IContactManager
    {
        //TODO: Remove the DDContactServiceUrl once refactored.
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        public IContactEndpointUtil EndpointUtil { get; set; }

       
        #region Contact
        #endregion

        #region CareTeam
        public CareTeam GetCareTeam(GetCareTeamRequest request)
        {
            CareTeam careTeam = null;
            try
            {
                CareTeamData careTeamData = EndpointUtil.GetCareTeam(request);
                if (careTeamData != null)
                {
                    careTeam = Mapper.Map<CareTeam>(careTeamData);
                }
                return careTeam;
            }
            catch (Exception ex) { throw ex; }
        }
        
        public SaveCareTeamResponse SaveCareTeam(SaveCareTeamRequest request)
        {
            var response = new SaveCareTeamResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            if (request.CareTeam == null)
                throw new ArgumentNullException("request.CareTeam");

            if(request.CareTeam.Members.IsNullOrEmpty())
                 throw new ApplicationException(string.Format("CareTeam should have atleast one or more members."));

            foreach (var member in request.CareTeam.Members)
            {
                ValidateCareTeamMemberFields(member);
            }

            try
            {
               var domainResponse =  EndpointUtil.SaveCareTeam(request);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SaveCareTeam()::" + ex.Message, ex.InnerException);
            }

            return response;

        }

        public UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request)
        {
            var response = new UpdateCareTeamMemberResponse();

            if (request == null)
                throw new ArgumentNullException("request");

            if (request.CareTeamMember == null)
                throw new ArgumentNullException("request.CareTeamMember");

            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers/{Id}", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/CareTeams/{5}/CareTeamMembers/{6}",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber, request.ContactId, request.CareTeamId, request.Id), request.UserId);
                var dataDomainResponse =
                    client.Put<UpdateCareTeamMemberResponse>(url, new UpdateCareTeamMemberDataRequest
                    {
                        CareTeamMemberData = Mapper.Map<CareTeamMemberData>(request.CareTeamMember),
                        Version = request.Version,
                        ContactId = request.ContactId,
                        CareTeamId = request.CareTeamId,
                        Id = request.Id,
                        ContractNumber = request.ContractNumber,
                        Context = "NG"
                    } as object);


            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:UpdateCareTeamMember()::" + wse.Message, wse.InnerException);
            }

            return response;
        }

        #endregion

        #region Private Methods
        private void ValidateCareTeamMemberFields(Member member)//, string contractNumber, string userId, double version)
        {
            if (member == null)
                throw new Exception("CareTeam Member cannot be null");

            if (string.IsNullOrEmpty(member.ContactId))
            {
                throw new Exception("ContactId cannot be null or empty");
            }
            else
            {
                //var response = GetContactByContactId(new GetContactByContactIdRequest
                //  {
                //      ContactId = member.ContactId,
                //      ContractNumber = contractNumber,
                //      UserId = userId,
                //      Version = version
                //  });

                //  if (response == null)
                //  {
                //      throw new Exception(string.Format("ContactId : {0} is not a valid contact",  member.ContactId));
                //  }
            }

            if (member.StatusId == 0 || member.StatusId == null)
            {
                throw new Exception("Status cannot be null");
            }

            if (string.IsNullOrEmpty(member.RoleId) && string.IsNullOrEmpty(member.CustomRoleName))
            {
                throw new Exception("Role or CustomRoleName is required");
            }

        }
        #endregion
    }
}
