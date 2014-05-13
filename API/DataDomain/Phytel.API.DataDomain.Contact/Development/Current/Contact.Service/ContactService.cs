using System;
using System.Net;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactService : ServiceStack.ServiceInterface.Service
    {
        public IContactDataManager Manager { get; set; }
        public IHelpers Helpers { get; set; }
        public ICommonFormatterUtil CommonFormat { get; set; }

        public GetContactDataResponse Get(GetContactDataRequest request)
        {
            GetContactDataResponse response = new GetContactDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Get()::Unauthorized Access");

                response.Contact = Manager.GetContactByPatientId(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetContactByUserIdDataResponse Get(GetContactByUserIdDataRequest request)
        {
            GetContactByUserIdDataResponse response = new GetContactByUserIdDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Get()::Unauthorized Access");

                response.Contact = Manager.GetContactByUserId(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public SearchContactsDataResponse Post(SearchContactsDataRequest request)
        {
            SearchContactsDataResponse response = new SearchContactsDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Post()::Unauthorized Access");

                response = Manager.SearchContacts(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutContactDataResponse Put(PutContactDataRequest request)
        {
            PutContactDataResponse response = new PutContactDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Put()::Unauthorized Access");

                response = Manager.InsertContact(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateContactDataResponse Put(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = new PutUpdateContactDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Put()::Unauthorized Access");

                response = Manager.UpdateContact(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllCareManagersDataResponse Get(GetAllCareManagersDataRequest request)
        {
            GetAllCareManagersDataResponse response = new GetAllCareManagersDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Get()::Unauthorized Access");

                response = Manager.GetCareManagers(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

    }
}