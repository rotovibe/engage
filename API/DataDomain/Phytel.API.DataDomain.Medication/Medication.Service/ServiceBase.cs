using System;
using System.Configuration;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class ServiceBase : ServiceStack.ServiceInterface.Service
    {
        public ICommonFormatterUtil FormatUtil { get; set; }
        public IHelpers Helpers { get; set; }
        protected readonly string _aseProcessId = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";

        protected static void RequireUserId(IDataDomainRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
                throw new UnauthorizedAccessException("MedicationDD:Put()::Unauthorized Access");
        }

        protected void RaiseException(IDomainResponse response, Exception ex)
        {
            FormatUtil.FormatExceptionResponse(response, base.Response, ex);
            Helpers.LogException(int.Parse(_aseProcessId), ex);
        }
    }
}