using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.AppDomain.NG
{
    public interface IContactEndpointUtil
    {
        #region Contact
        ContactData GetContactByContactId(GetContactByContactIdRequest request);
        List<ContactData> GetContactsByContactIds(List<string> contactIds, double version, string contractNumber,string userId);
        List<Contact> GetCareManagers(GetAllCareManagersRequest request);
        DereferencePatientDataResponse DereferencePatientInContact(string patientId, double version, string contractNumber, string userId);
        bool UndoDereferencePatientInContact(string contactId,string patientId, double version, string contractNumber, List<ContactWithUpdatedRecentList> contactWithUpdatedRecentLists, string userId);

        #endregion

        #region CareTeam
        CareTeamData GetCareTeam(GetCareTeamRequest request);
        SaveCareTeamDataResponse SaveCareTeam(SaveCareTeamRequest request);
        DeleteCareTeamMemberDataResponse DeleteCareTeamMember(DeleteCareTeamMemberRequest request);
        UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request);

        DeleteCareTeamDataResponse DeleteCareTeam(DeleteCareTeamRequest request);
        UndoDeleteCareTeamDataResponse UndoDeleteCareTeam(UndoDeleteCareTeamDataRequest request);
        AddCareTeamMemberDataResponse AddCareTeamMember(AddCareTeamMemberRequest request);

        #endregion

        #region CohortPatientView

        bool AddPCMToCohortPatientView(string patientId, string contactIdToAdd,double version, string contractNumber,string userId,bool activeCorePcmIsUser);
        bool RemovePCMCohortPatientView(string patientId, double version, string contractNumber, string userId);
        bool AssignContactsToCohortPatientView(string patientId,List<string> contactIds , double version, string contractNumber, string userId);

        #endregion

    }
}
