using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace Phytel.API.AppDomain.NG
{    
    public class CareTeamCommand : INGCommand
    {
        private CareTeam _careTeam;
        private readonly PostDeletePatientRequest _request;
        public IContactEndpointUtil ContactEndpointUtil { get; set; }
        public INGManager NgManager { get; set; }
        public CareTeamCommand(PostDeletePatientRequest req)
        {
            _request = req as PostDeletePatientRequest;           
        }
    public void Execute()
        {
            try
            {
                // We get the patient contact card
                var getContactByPatientId = new GetContactByPatientIdRequest()
                {
                    PatientID = _request.Id,
                    ContractNumber = _request.ContractNumber,
                    UserId = _request.UserId,
                    Version = _request.Version
                };
                var patientContactCard = NgManager.GetContactByPatientId(getContactByPatientId);

                // We get the patient care team using the patient contact's Id
                if (patientContactCard==null)
                    throw new Exception("The patient contact card was not found");
                
                var getCareTeamRequest = new GetCareTeamRequest()
                {
                    ContactId = patientContactCard.Id,
                    ContractNumber = _request.ContractNumber,
                    UserId = _request.UserId,
                    Version = _request.Version
                };
                var careTeamData = ContactEndpointUtil.GetCareTeam(getCareTeamRequest);

                if (careTeamData==null)
                    throw new Exception("The patient care team was not found");

                _careTeam = Mapper.Map<CareTeam>(careTeamData);
                                              
                // We send a request to the data domain to delete care team by care tam Id  
                var request = new DeleteCareTeamRequest()
                {
                    ContactId = _careTeam.ContactId,
                    Id = _careTeam.Id,
                    ContractNumber = _request.ContractNumber,
                    UserId = _request.UserId,
                    Version = _request.Version
                };
                ContactEndpointUtil.DeleteCareTeam(request);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: CareTeamCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                if (_careTeam==null)
                    throw new Exception("The deleted Patient Care Team was not found");
                var request = new UndoDeleteCareTeamDataRequest()
                {
                    ContactId = _careTeam.ContactId,
                    Id = _careTeam.Id,
                    ContractNumber = _request.ContractNumber,                    
                    UserId = _request.UserId,
                    Version = _request.Version
                };
                ContactEndpointUtil.UndoDeleteCareTeam(request);
            }
            catch (Exception ex)
            {
                    
                throw new Exception("AD: CareTeamCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
