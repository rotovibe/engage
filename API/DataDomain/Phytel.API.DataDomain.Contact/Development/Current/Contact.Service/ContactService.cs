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
            response.Version = request.Version;
            try
            {
                 response.Contact = ContactDataManager.GetContactByPatientId(request);
                 
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }


        public PutContactDataResponse Put(PutContactDataRequest request)
        {
            PutContactDataResponse response = new PutContactDataResponse();
            response.Version = request.Version;
            try
            {
                response = ContactDataManager.InsertContact(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutUpdateContactDataResponse Put(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = new PutUpdateContactDataResponse();
            response.Version = request.Version;
            try
            {
                response = ContactDataManager.UpdateContact(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

    }
}