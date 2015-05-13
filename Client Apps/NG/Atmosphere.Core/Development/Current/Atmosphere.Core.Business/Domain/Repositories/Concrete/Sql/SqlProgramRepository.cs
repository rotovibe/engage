using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using C3.Data;
using C3.Data.Enum;
using C3.Domain.Repositories.Abstract;

namespace C3.Domain.Repositories.Concrete.Sql
{
    public class SqlProgramRepository : IProgramRepository
    {
        public IList<Data.Program> GetReportFilters(int contractId, List<Data.Group> groups, MeasureTypes type)
        {
            var context = new SqlComparisonDataContext();
            var programs = (from p in context.Programs
                                           from cgp in context.ContractGroupPrograms
                                           from cg in context.ContractGroups
                                           from c in context.Contracts
                                           from g in context.Groups
                                           from pc in context.ProgramConditions
                            where c.ContractId == contractId
                                  && c.ContractId == cg.ContractId
                                  && cg.ContractGroupId == cgp.ContractGroupId
                                  && cgp.ProgramId == p.ProgramId
                                  && p.ProgramId == pc.ProgramId
                            select new Data.Program
                            {
                                Name = p.ProgramName,
                                ProgramId = p.ProgramId, 
                            }).Distinct().OrderBy(p => p.Name).ToList();
            foreach(Data.Program program in programs)
            {
                int programId = program.ProgramId;
                program.Groups = (from cgp in context.ContractGroupPrograms
                            from cg in context.ContractGroups
                            from g in context.Groups
                            where cgp.ProgramId == programId
                                  && cgp.ContractGroupId == cg.ContractGroupId
                                  && cg.GroupId == g.GroupId
                            select new Data.Group() {GroupId = g.GroupId, Name = g.Name}).Distinct().OrderBy(g => g.Name)
                    .ToList();
                foreach (Data.Group group in program.Groups)
                {
                    int groupId = group.GroupId;
                    group.Subscribers = (from sb in context.GroupSubscribers
                                             where sb.GroupId == groupId
                                             select
                                                 new Data.Subscriber
                                                 {
                                                     SubscriberId = sb.SubscriberId,
                                                     Name = sb.Subscriber.SubscriberName
                                                 }).Distinct().OrderBy(
                                                         p => p.Name).ToList();
                }
                program.Conditions = (from pc in context.ProgramConditions
                                where pc.ProgramId == programId
                                select new Data.Condition{ConditionId = pc.ConditionId, Name = pc.Condition.ConditionName}).Distinct().OrderBy(c=>c.Name).ToList();                                   
                foreach(Data.Condition condition in program.Conditions)
                {
                    int conditionId = condition.ConditionId;
                    condition.Populations = (from cp in context.ConditionPopulations
                                             where cp.ConditionId == conditionId
                                             select
                                                 new Data.Population
                                                     {
                                                         PopulationId = cp.PopulationId,
                                                         Name = cp.Population.PopulationName
                                                     }).Distinct().OrderBy(
                                                         p => p.Name).ToList();
                    foreach(Data.Population population in condition.Populations)
                    {
                        int populationId = population.PopulationId;
                        population.Measures = (from m in context.Measures
                                               where
                                                   m.ConditionPopulation.PopulationId == populationId &&
                                                   m.ConditionPopulation.ConditionId == conditionId
                                               select new Data.Measure
                                                          {
                                                              MeasureId = m.MeasureId,
                                                              Name = m.MeasureName
                    }).Distinct().OrderBy(m=>m.Name).ToList();
                    }
                }
            }
            return programs;
        }
    }
}
