using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.CareTeam;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact
{
    public class CommonDataManager : ICommonDataManager
    {
        
        private readonly IContactRepositoryFactory _contactRepositoryFactory;
        private readonly ICareTeamRepositoryFactory _careTeamRepositoryFactory;

        public CommonDataManager(IContactRepositoryFactory contactRepositoryFactory, ICareTeamRepositoryFactory careTeamRepositoryFactory)
        {
          if(contactRepositoryFactory == null)
             throw new ArgumentNullException("contactRepositoryFactory");

            if(careTeamRepositoryFactory == null)
                throw new ApplicationException("careTeamRepositoryFactory");


            _contactRepositoryFactory = contactRepositoryFactory;
            _careTeamRepositoryFactory = careTeamRepositoryFactory;
          

        }

        public GetPatientsCareTeamInfoResponse GetPatientsCareTeamInfo(DTO.GetPatientsCareTeamInfoRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            var contactRepository = _contactRepositoryFactory.GetRepository(request, RepositoryType.Contact);
            var careTeamRepository = _careTeamRepositoryFactory.GetCareTeamRepository(request, RepositoryType.CareTeam);
            
            var response = new GetPatientsCareTeamInfoResponse();
            try
            {
                var contacts = contactRepository.GetContactsByPatientIds(request.PatientIds);
                

                if (contacts.IsNullOrEmpty())
                    return response;

                var contactIds = contacts.Select(c => c.Id).ToList();

                //Fetch contacts
                var mappedContacts = new List<PatientCareTeamInfo>();
                foreach (var contact in contacts)
                {
                    var mappedContact = new PatientCareTeamInfo
                    {
                        ContactId = contact.Id,
                        PatientId = contact.PatientId
                    };

                    mappedContacts.Add(mappedContact);
                }

                //Fetch CareTeams
                var careTeams = careTeamRepository.GetCareTeamsByContactIds(contactIds);
                if (!careTeams.IsNullOrEmpty())
                {
                    foreach (var mc in mappedContacts)
                    {
                        var contactId = mc.ContactId;
                        if (contactId != null)
                        {
                            var careTeam = careTeams.FirstOrDefault(c => c.ContactId == contactId);

                            if (careTeam != null)
                            {
                                mc.CareTeamId = careTeam.Id;
                            }
                        }
                    }
                }

                response.ContactCareTeams = mappedContacts;

            }
            catch (Exception ex) 
            {
                    
                throw ex;
            }

            return response;
        }
    }
}
