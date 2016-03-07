using System;
using Funq;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.Interface;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceContext>(c =>
                new ServiceContext
                {
                    Contract = HostContext.Instance.Items["Contract"].ToString(),
                    Version = Convert.ToDouble(HostContext.Instance.Items["Version"].ToString())
                }).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<AuditHelpers, IAuditHelpers>().ReusedWithin(ReuseScope.Request);
            container.Register<IHelpers>(c => new Helpers()).ReusedWithin(ReuseScope.Request);

            container.Register<IReferralRepository<IDataDomainRequest>>(c => 
                new MongoReferralRepository<IDataDomainRequest>(c.Resolve<IServiceContext>().Contract))
                .ReusedWithin(ReuseScope.Request);

            container.Register<IDataReferralManager>(c => 
                new DataReferralManager(c.Resolve<IServiceContext>(), c.Resolve<IReferralRepository<IDataDomainRequest>>()))
                .ReusedWithin(ReuseScope.Request);

            return container;
        }
    }
}