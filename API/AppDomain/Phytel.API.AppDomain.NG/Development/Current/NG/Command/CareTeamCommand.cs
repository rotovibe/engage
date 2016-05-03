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
        private IContactEndpointUtil _contactEndpointUtil;
        public INGManager NgManager { get; set; }
        private string _contactId;
        public CareTeamCommand(PostDeletePatientRequest req,IContactEndpointUtil contactEndpointUtil, string contactId)
        {
            _request = req as PostDeletePatientRequest;           
        }
    public void Execute()
        {
            try
            {
                
                var getCareTeamRequest = new GetCareTeamRequest()
                {
                    ContactId = _contactId,
                    ContractNumber = _request.ContractNumber,
                    UserId = _request.UserId,
                    Version = _request.Version
                };
                var careTeamData = _contactEndpointUtil.GetCareTeam(getCareTeamRequest);

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
                _contactEndpointUtil.DeleteCareTeam(request);
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
                _contactEndpointUtil.UndoDeleteCareTeam(request);
            }
            catch (Exception ex)
            {
                    
                throw new Exception("AD: CareTeamCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
