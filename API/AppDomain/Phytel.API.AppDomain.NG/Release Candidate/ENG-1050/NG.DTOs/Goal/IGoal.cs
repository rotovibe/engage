using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Goal
{
    public interface IGoal
    {
        string Id { get; set; }
        List<string> FocusAreaIds { get; set; }
        string Name { get; set; }
        string SourceId { get; set; }
        int TypeId { get; set; }
        int StatusId { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        int StartDateRange { get; set; }
        int TargetDateRange { get; set; }
        string TargetValue { get; set; }
        DateTime? TargetDate { get; set; }
        List<CustomAttribute> CustomAttributes { get; set; }
    }
}