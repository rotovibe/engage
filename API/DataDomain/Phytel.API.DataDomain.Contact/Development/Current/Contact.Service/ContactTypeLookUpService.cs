using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactTypeLookUpService : ServiceStack.ServiceInterface.Service
    {

        public IHelpers Helpers { get; set; }
        public ICommonFormatterUtil CommonFormat { get; set; }

        public GetContactTypeLookUpDataResponse Get(GetContactTypeLookUpDataRequest request)
        {
            var response = new GetContactTypeLookUpDataResponse { Version = request.Version };
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ContactDD:ContactTypeLookUpService:Get()::Unauthorized Access");


                var fakeLookups = new List<ContactTypeLookUpData>()
                {
                    new ContactTypeLookUpData 
                    { 
                        Id = "56ea0c2c64e91cf53bbfca5f",
                        Name = "Doctor",
                        Role = "Doctor(M.D)",
                        CreatedOn = DateTime.UtcNow,
                        Group = ContactLookUpGroupType.IndividualTypes,
                        Children = new List<ContactTypeLookUpData>
                        {
                            new ContactTypeLookUpData
                            {
                                Id = "56ea228e64e91cf53bbfca66",
                                Name = "Addiction Medicine",
                                Role = "Addition Medicine",
                                CreatedOn = DateTime.UtcNow,
                                Group = ContactLookUpGroupType.IndividualTypes
                            }
                        }
                    }
                };

                response.ContactTypeLookUps = fakeLookups;

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