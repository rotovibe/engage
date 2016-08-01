using System;
using System.Configuration;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Contact.ContactTypeLookUp;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactTypeLookUpService : ServiceStack.ServiceInterface.Service
    {
        public IContactTypeLookUpManager Manager { get; set; }
        public IHelpers Helpers { get; set; }
        public ICommonFormatterUtil CommonFormat { get; set; }

        public GetContactTypeLookUpDataResponse Get(GetContactTypeLookUpDataRequest request)
        {
            var response = new GetContactTypeLookUpDataResponse { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:ContactTypeLookUpService:Get()::Unauthorized Access");

                var lookups = Manager.GetContactTypeLookUps(request);

                response.ContactTypeLookUps = lookups.ContactTypeLookUps;

            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutContactTypeLookUpDataResponse Put(PutContactTypeLookUpDataRequest request)
        {
            var response = new PutContactTypeLookUpDataResponse { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:ContactTypeLookUpService:Put()::Unauthorized Access");

                var dataResponse = Manager.SavContactTypeLookUp(request);

                response.Id = dataResponse.Id;

            }
            catch (Exception ex)
            {
                CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }


    }
}