using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG.Command
{
    public class DereferencePatientInContactCommand : INGCommand
    {
        private readonly string _contactId;
        private readonly PostDeletePatientRequest _request;
        private readonly IContactEndpointUtil _contactEndpointUtil;       
        private List<ContactWithUpdatedRecentList> contactWithUpdatedRecentLists;
        public DereferencePatientInContactCommand(string contactId ,PostDeletePatientRequest request, IContactEndpointUtil contactEndpointUtil)
        {
            if(contactId == null)
                throw new ArgumentNullException("contactId");

            if(request == null)
                throw new ArgumentNullException("request");

            if(contactEndpointUtil == null)
                throw new ArgumentNullException("contactEndpointUtil");

            _contactId = contactId;
            _request = request;
            _contactEndpointUtil = contactEndpointUtil;
        }

        public void Execute()
        {
            try
            {
                var patientId = _request.Id;
                var response = _contactEndpointUtil.DereferencePatientInContact(patientId, _request.Version, _request.ContractNumber, _request.UserId);
                if (response!=null)
                {                
                    contactWithUpdatedRecentLists = response.ContactWithUpdatedRecentLists;
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception("AD: DereferencePatientInContactCommand Execute::" + ex.Message, ex.InnerException);
            }
            
        }

        public void Undo()
        {
            try
            {
                var patientId = _request.Id;
                var contactId = _contactId;
                _contactEndpointUtil.UndoDereferencePatientInContact(contactId,patientId, _request.Version, _request.ContractNumber,contactWithUpdatedRecentLists, _request.UserId);

            }
            catch (Exception ex)
            {

                throw new Exception("AD: DereferencePatientInContactCommand Execute::" + ex.Message, ex.InnerException);
            }
        }
    }
}
