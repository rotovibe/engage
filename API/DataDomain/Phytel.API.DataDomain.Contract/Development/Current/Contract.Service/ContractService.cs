using System;
using System.Net;
using Phytel.API.DataDomain.Contract.Repository;
using Phytel.API.DataDomain.Contract.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Contract.Service
{
    public class ContractService : ServiceStack.ServiceInterface.Service
    {
        public IContractDataManager Manager { get; set; }
        //public IHelpers Helpers { get; set; }
        //public ICommonFormatterUtil CommonFormat { get; set; }

        public GetAllContractsDataResponse Get(GetAllContractsDataRequest request)
        {
            GetAllContractsDataResponse response = new GetAllContractsDataResponse();
            response.Version = request.Version;
            try
            {
                //if (string.IsNullOrEmpty(request.UserId))
                //    throw new UnauthorizedAccessException("ContractDD:Get()::Unauthorized Access");

                response.Contracts = Manager.GetAllContracts(request);
            }
            catch (Exception ex)
            {
                //CommonFormat.FormatExceptionResponse(response, base.Response, ex);

                //string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                //Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}