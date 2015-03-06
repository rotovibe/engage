using System;
namespace Phytel.API.AppDomain.NG.Programs
{
    public interface IPlanManager
    {
        IPlanElementUtils PEUtils { get; set; }
        Phytel.API.AppDomain.NG.DTO.PostProcessActionResponse ProcessActionResults(Phytel.API.AppDomain.NG.DTO.PostProcessActionRequest request);
        System.Collections.Generic.List<object> ProcessedElements { get; set; }
        System.Collections.Generic.List<string> RelatedChanges { get; set; }
        Phytel.API.AppDomain.NG.DTO.PostSaveActionResponse SaveActionResults(Phytel.API.AppDomain.NG.DTO.PostSaveActionRequest request);
    }
}
