using System;
using System.Collections.Generic;
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

        public GetContactByPatientIdDataResponse Get(GetContactByPatientIdDataRequest request)
        {
            GetContactByPatientIdDataResponse response = new GetContactByPatientIdDataResponse();
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

        public GetContactByContactIdDataResponse Get(GetContactByContactIdDataRequest request)
        {
            GetContactByContactIdDataResponse response = new GetContactByContactIdDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Get()::Unauthorized Access");

                response = Manager.GetContactByContactId(request);
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

        public GetContactsByContactIdsDataResponse Post(GetContactsByContactIdsDataRequest request)
        {
            var response = new GetContactsByContactIdsDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Post()::Unauthorized Access");

                response = Manager.GetContactsByContactId(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public InsertBatchContactDataResponse Post(InsertBatchContactDataRequest request)
        {
            InsertBatchContactDataResponse response = new InsertBatchContactDataResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Post()::Unauthorized Access");

                response.Responses = Manager.InsertBatchContacts(request);
            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public InsertContactDataResponse Post(InsertContactDataRequest request)
        {
            InsertContactDataResponse response = new InsertContactDataResponse();
            if (request != null)
            {
                response.Version = request.Version;
                try
                {
                    if (string.IsNullOrEmpty(request.UserId))
                        throw new UnauthorizedAccessException("ContactDD:Post()::Unauthorized Access");

                    response.Id = Manager.InsertContact(request);
                }
                catch (Exception ex)
                {
                    CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                    string aseProcessId = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                    Helpers.LogException(int.Parse(aseProcessId), ex);
                }
            }
            return response;
        }

        public UpdateContactDataResponse Put(UpdateContactDataRequest request)
        {
            UpdateContactDataResponse response = new UpdateContactDataResponse();
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

        public PutRecentPatientResponse Put(PutRecentPatientRequest request)
        {
            PutRecentPatientResponse response = new PutRecentPatientResponse();
            response.Version = request.Version;
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:Put()::Unauthorized Access");

                response = Manager.AddRecentPatient(request);
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

        public DeleteContactByPatientIdDataResponse Delete(DeleteContactByPatientIdDataRequest request)
        {
            DeleteContactByPatientIdDataResponse response = new DeleteContactByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:ContactDelete()::Unauthorized Access");

                response = Manager.DeleteContactByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public UndoDeleteContactDataResponse Put(UndoDeleteContactDataRequest request)
        {
            UndoDeleteContactDataResponse response = new UndoDeleteContactDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:ContactUndoDelete()::Unauthorized Access");

                response = Manager.UndoDeleteContact(request);
                response.Version = request.Version;
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
            
            if(request == null)
                throw new ArgumentNullException("request");
            var response = new SearchContactsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:SearchContactsDataRequest()::Unauthorized Access");

                response = Manager.SearchContacts(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }

            return response;
        }

        public SyncContactInfoDataResponse Put(SyncContactInfoDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            var response = new SyncContactInfoDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:SyncContactInfoDataResponse()::Unauthorized Access");

                response = Manager.SyncContactInfo(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DereferencePatientDataResponse Put(DereferencePatientDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            var response = new DereferencePatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:DereferencePatientDataResponse()::Unauthorized Access");

                response = Manager.DereferencePatient(request);
                response.Version = request.Version;
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