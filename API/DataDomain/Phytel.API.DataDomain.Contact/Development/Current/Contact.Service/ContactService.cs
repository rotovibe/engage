using System;
using System.Net;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactService : ServiceStack.ServiceInterface.Service
    {
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public GetContactDataResponse Get(GetContactDataRequest request)
        {
            GetContactDataResponse response = new GetContactDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response.Contact = ContactDataManager.GetContactByPatientId(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetContactByUserIdDataResponse Get(GetContactByUserIdDataRequest request)
        {
            GetContactByUserIdDataResponse response = new GetContactByUserIdDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response.Contact = ContactDataManager.GetContactByUserId(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public SearchContactsDataResponse Post(SearchContactsDataRequest request)
        {
            SearchContactsDataResponse response = new SearchContactsDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response = ContactDataManager.SearchContacts(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutContactDataResponse Put(PutContactDataRequest request)
        {
            PutContactDataResponse response = new PutContactDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response = ContactDataManager.InsertContact(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateContactDataResponse Put(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = new PutUpdateContactDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response = ContactDataManager.UpdateContact(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllCareManagersDataResponse Get(GetAllCareManagersDataRequest request)
        {
            GetAllCareManagersDataResponse response = new GetAllCareManagersDataResponse();
            response.Version = request.Version;
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                response = ContactDataManager.GetCareManagers(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

    }
}