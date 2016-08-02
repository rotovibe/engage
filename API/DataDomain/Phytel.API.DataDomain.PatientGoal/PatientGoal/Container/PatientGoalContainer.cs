using System;
using AutoMapper;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Container
{
    public static class PatientGoalContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            Mapper.CreateMap<MEGoal, GoalData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(d => d.SourceId, opt => opt.MapFrom(src => src.SourceId.ToString()))
                .ForMember(d => d.TypeId,
                    opt => opt.MapFrom(src => Convert.ChangeType(src.Type, src.Status.GetTypeCode())))
                .ForMember(d => d.StatusId,
                    opt => opt.MapFrom(src => Convert.ChangeType(src.Status, src.Status.GetTypeCode())))
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.Attributes.ConvertAll<CustomAttributeData>(
                        c => new CustomAttributeData {Id = c.AttributeId.ToString(), Values = c.Values})));

            return container;
        }
    }
}
