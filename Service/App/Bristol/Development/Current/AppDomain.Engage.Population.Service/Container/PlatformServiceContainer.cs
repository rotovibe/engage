using Funq;
using Phytel.API.AppDomain.Platform.Security.DTO.Client;
using Phytel.API.ASE.Client;
using Phytel.API.ASE.Client.Interface;
using Phytel.Services.API;
using Phytel.Services.API.Platform;
using Phytel.Services.API.Platform.Filter;
using Phytel.Services.API.Provider;

namespace AppDomain.Engage.Population.Service.Container
{
    public class PlatformServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            #region ASE Client
            if (container.TryResolve<IASEClient>() == null)
            {
                // get from services file
                var aseClientUrl = APIService.Instance.GetURL("ASEAPI"); 
                container.Register<Phytel.API.ASE.Client.Interface.IRepositoryHttp>("ASE", c => new Phytel.API.ASE.Client.RepositoryHttp(aseClientUrl, 30)).ReusedWithin(ReuseScope.Request);
                container.Register<IASEClient>(c => new ASEClient(c.ResolveNamed<Phytel.API.ASE.Client.Interface.IRepositoryHttp>("ASE"))).ReusedWithin(ReuseScope.Request);
            }
            #endregion

            #region App Domain
            var appDomainUrl = APIService.Instance.GetURL("PlatformAppDomainUrl"); 
            container.Register<Phytel.Services.API.IRepositoryHttp>("APPDOMAIN", c => new Phytel.Services.API.RepositoryHttp(appDomainUrl, 30)).ReusedWithin(ReuseScope.Request);
            container.Register<IPlatformTokenClient>(c => new PlatformTokenClient(c.ResolveNamed<Phytel.Services.API.IRepositoryHttp>("APPDOMAIN"))).ReusedWithin(ReuseScope.Request);
            #endregion

            container.Register<ITokenManager>(t => new TokenManager(container.Resolve<IASEClient>(), container.Resolve<IPlatformTokenClient>())).ReusedWithin(ReuseScope.Request);
            container.Register<IAuditLogger>(log => new AuditLogger(container.Resolve<IASEClient>(), container.Resolve<ITokenManager>()));
            container.Register<IHostContextProxy>(p => new HostContextProxy());

            return container;
        }
    }
}