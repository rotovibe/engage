using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG
{
    public class ContactManager : ManagerBase, IContactManager
    {
        
        public IContactEndpointUtil EndpointUtil { get; set; }
        public ICohortRulesProcessor CohortRules { get; set; }
        public ICareMemberCohortRuleFactory CareMemberCohortRuleFactory { get; set; }
        public ICohortRuleUtil CohortRuleUtil { get; set; }

        #region Contact
        public Contact GetContactByContactId(GetContactByContactIdRequest request)
        {
            Contact contact = null;
            try
            {
                ContactData contactData = EndpointUtil.GetContactByContactId(request);
                if (contactData != null)
                {
                    contact = Mapper.Map<Contact>(contactData);
                    if (!string.IsNullOrEmpty(contactData.PatientId))
                        contact.IsPatient = true;

                    if (!string.IsNullOrEmpty(contactData.UserId))
                        contact.IsUser = true;
                }
                return contact;
            }
            catch (Exception ex) { throw ex; }

        }
        #endregion

        #region CareTeam
        public CareTeam GetCareTeam(GetCareTeamRequest request)
        {
            CareTeam careTeam = null;
            if (request == null)
                throw new ArgumentNullException("request");

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("request.ContactId");
            try
            {
                CareTeamData careTeamData = EndpointUtil.GetCareTeam(request);
                if (careTeamData != null)
                {
                    careTeam = Mapper.Map<CareTeam>(careTeamData);
                    #region Populate Contact object for each care team member.
                    if (careTeam.Members != null && careTeam.Members.Count > 0)
                    {
                        List<string> contactIds = careTeam.Members.Select(a => a.ContactId).ToList();
                        List<ContactData> contactsData = EndpointUtil.GetContactsByContactIds(contactIds, request.Version, request.ContractNumber, request.UserId);
                        if (contactsData != null)
                        {
                            foreach (var member in careTeam.Members)
                            {
                                ContactData data = contactsData.FirstOrDefault(c => c.Id == member.ContactId);
                                if (data == null)
                                {
                                    throw new ApplicationException(string.Format("Contact card for a care team member with contact id = {0} was not found", member.ContactId));
                                }
                                else
                                {
                                    member.Contact = Mapper.Map<Contact>(data);
                                }
                            }
                        }
                    }
                    #endregion
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


            if (CohortRuleUtil.HasMultipleActiveCorePCM(request.CareTeam))
                throw new ApplicationException("The Care team cannot have multiple Active, Core PCMs");

            if (CohortRuleUtil.HasMultipleActiveCorePCP(request.CareTeam))
                throw new ApplicationException("The Care team cannot have multiple Active, Core PCPs");


            foreach (var member in request.CareTeam.Members)
            {
                ValidateCareTeamMemberFields(member);
            }

            try
            {
               var domainResponse =  EndpointUtil.SaveCareTeam(request);

              // OnCareMemberUpdated(request.ContactId,request.ContractNumber,request.UserId);
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
                throw new ArgumentNullException("CareTeamMemberData is Null","request");

            if (string.IsNullOrEmpty(request.ContactId))
                throw new ArgumentNullException("Null or empty ContactId", "request");

            if (string.IsNullOrEmpty(request.CareTeamId))
                throw new ArgumentNullException("Null or empty CareTeamId", "request");

            if (string.IsNullOrEmpty(request.Id))
                throw new ArgumentNullException("Null or empty MemberId", "request");

            if (request.Id != request.CareTeamMember.Id)
                throw new ArgumentNullException("CareTeamMemberData.Id and Id are different", "request");

            var cohortRuleCheckData = new CohortRuleCheckData()
            {
                ContactId = request.ContactId,
                ContractNumber = request.ContractNumber,
                UserId = request.UserId
            };

            string currentActiveCorePCMId = CohortRules.GetCareTeamActiveCorePCMId(cohortRuleCheckData);
            if (currentActiveCorePCMId!=null && currentActiveCorePCMId!=request.Id)
                throw new ArgumentNullException("Care Team already has an Active Core PCM", "request");

            ValidateCareTeamMemberFields(request.CareTeamMember);
            
            try
            {
                var domainResponse = EndpointUtil.UpdateCareTeamMember(request);
                if (domainResponse != null)
                {
                    response.Status = domainResponse.Status;                   
                    CohortRules.EnqueueCohorRuleCheck(cohortRuleCheckData);                   
                    //OnCareMemberUpdated(request.ContactId, request.ContractNumber, request.UserId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:UpdateCareTeamMember()::" + ex.Message, ex.InnerException);
            }
           
            return response;
        }

        
        public DeleteCareTeamMemberResponse DeleteCareTeamMember(DeleteCareTeamMemberRequest request)
        {
            var response = new DeleteCareTeamMemberResponse();

            if (request == null)
                throw new ArgumentNullException("request");
            try
            {
                EndpointUtil.DeleteCareTeamMember(request);
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:DeleteCareTeamMemberResponse()::" + wse.Message, wse.InnerException);
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

        //private void OnCareMemberUpdated(string contactId, string contractNumber,string userId)
        //{
        //    var careTeam =  GetCareTeam(new GetCareTeamRequest
        //    {
        //        ContactId = contactId, 
        //        ContractNumber = contractNumber, 
        //        UserId = userId
        //    });
        //    //if (careTeam!=null)
        //    //{
        //    //    CohortRules.EnqueueCareTeam(careTeam);
        //    //}
        //    var rules = CareMemberCohortRuleFactory.GenerateEngageCareMemberCohortRules();
        //    foreach (var rule in rules)
        //    {
        //        rule.Run(careTeam);
        //    }

        //}
        #endregion
        
    }
}
