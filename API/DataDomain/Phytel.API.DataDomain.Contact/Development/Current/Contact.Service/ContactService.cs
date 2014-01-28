using System;
using System.Net;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactService : ServiceStack.ServiceInterface.Service
    {

        public GetContactDataResponse Get(GetContactDataRequest request)
        {
            GetContactDataResponse response = new GetContactDataResponse();
            try
            {
                 response.Contact = ContactDataManager.GetContactByPatientId(request);
                 response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

    }
}