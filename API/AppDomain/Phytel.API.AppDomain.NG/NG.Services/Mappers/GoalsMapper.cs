using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class GoalsMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<GoalData, Goal>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            Mapper.CreateMap<TaskData, Task>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            Mapper.CreateMap<Task, PatientTask>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            Mapper.CreateMap<PatientTaskData, PatientTask>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            Mapper.CreateMap<InterventionData, Intervention>();

            // goal to patientgoal
            Mapper.CreateMap<Goal, PatientGoal>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            Mapper.CreateMap<PatientInterventionData, PatientIntervention>();

            Mapper.CreateMap<PatientGoalData, PatientGoal>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })))
                .ForMember(d => d.Barriers,
                    opt => opt.MapFrom(src => src.BarriersData.ConvertAll(
                        c =>
                            new PatientBarrier
                            {
                                Id = c.Id,
                                CategoryId = c.CategoryId,
                                DeleteFlag = c.DeleteFlag,
                                Name = c.Name,
                                PatientGoalId = c.PatientGoalId,
                                StatusDate = c.StatusDate,
                                StatusId = c.StatusId,
                                Details = c.Details
                            })))
                .ForMember(d => d.Tasks,
                    opt => opt.MapFrom(src => src.TasksData.ConvertAll(
                        c =>
                            new PatientTask
                            {
                                Id = c.Id,
                                BarrierIds = c.BarrierIds,
                                ClosedDate = c.ClosedDate,
                                CreatedById = c.CreatedById,
                                CustomAttributes =
                                    c.CustomAttributes.ConvertAll(
                                        ca =>
                                            new CustomAttribute
                                            {
                                                ControlType = ca.ControlType,
                                                Id = ca.Id,
                                                Name = ca.Name,
                                                Order = ca.Order,
                                                Required = ca.Required,
                                                Type = ca.Type,
                                                Values = ca.Values,
                                                Options = NGUtils.FormatOptions(ca.Options)
                                            }),
                                DeleteFlag = c.DeleteFlag,
                                Description = c.Description,
                                GoalName = c.GoalName,
                                PatientGoalId = c.PatientGoalId,
                                StartDate = c.StartDate,
                                StatusDate = c.StatusDate,
                                StatusId = c.StatusId,
                                TargetDate = c.TargetDate,
                                TargetValue = c.TargetValue,
                                Details = c.Details
                            })))
                .ForMember(d => d.Interventions,
                    opt => opt.MapFrom(src => src.InterventionsData.ConvertAll(
                        c =>
                            new PatientIntervention
                            {
                                AssignedToId = c.AssignedToId,
                                BarrierIds = c.BarrierIds,
                                CategoryId = c.CategoryId,
                                ClosedDate = c.ClosedDate,
                                CreatedById = c.CreatedById,
                                DeleteFlag = c.DeleteFlag,
                                Description = c.Description,
                                GoalName = c.GoalName,
                                Id = c.Id,
                                PatientGoalId = c.PatientGoalId,
                                PatientId = c.PatientId,
                                StartDate = c.StartDate,
                                DueDate = c.DueDate,
                                StatusDate = c.StatusDate,
                                StatusId = c.StatusId,
                                Details = c.Details
                            })));
        }
    }
}