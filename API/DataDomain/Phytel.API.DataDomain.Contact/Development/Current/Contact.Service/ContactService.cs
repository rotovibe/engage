using System;
using System.Net;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactService : ServiceStack.ServiceInterface.Service
    {
        public GetContactResponse Post(GetContactRequest request)
        {
            GetContactResponse response = new GetContactResponse();
            try
            {
                response = ContactDataManager.GetContactByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public GetContactResponse Get(GetContactRequest request)
        {
            GetContactResponse response = new GetContactResponse();
            try
            {
             response = ContactDataManager.GetContactByID(request);
            response.Version = request.Version;
                        }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public GetAllContactsResponse Post(GetAllContactsRequest request)
        {
            GetAllContactsResponse response = new GetAllContactsResponse();
            try
            {
                response = ContactDataManager.GetContactList(request);
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