using System;
using System.Data;
using System.Collections.Generic;

using C3.Business.Interfaces;
using C3.Data;


namespace C3.Business
{
    public class ProgramService : ServiceBase, IProgramService
    {
        private static volatile ProgramService _instance;
        private static object _syncRoot = new Object();

        //FF: I made this public for Dependency Injection Implementation.
        public ProgramService()
        {

        }

        public static ProgramService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if ( _instance == null)
                            _instance = new ProgramService();
                    }
                }
                return _instance;
            }
        }

        public List<Program> GetReportPrograms(int contractId, int? measureTypeId, string groupIds)
        {
            List<Program> programs = new List<Program>();

            Contract contract = ContractService.Instance.GetContractById(contractId);

            DataSet dsReportFilters = QueryDataSet(contract.ConnectionString, contract.Database, StoredProcedure.GetProgramAndRelatedReportFilters, groupIds, measureTypeId);

            dsReportFilters.Tables[0].TableName = "Programs";
            dsReportFilters.Tables[1].TableName = "Groups";
            dsReportFilters.Tables[2].TableName = "Conditions";
            dsReportFilters.Tables[3].TableName = "Populations";
            dsReportFilters.Tables[4].TableName = "Measures";
            dsReportFilters.Tables[5].TableName = "Subscribers";
            dsReportFilters.Tables[6].TableName = "Payers";
            dsReportFilters.Tables[7].TableName = "ReportDate";
            dsReportFilters.Tables[8].TableName = "AttributionTypes";

            //Build Programs out based upon dataset table.
            if (dsReportFilters.Tables["Programs"].Rows.Count > 0)
            {
                foreach (DataRow row in dsReportFilters.Tables["Programs"].Rows)
                {
                    Program program = new Program();

                    program.ProgramId = Convert.ToInt32(row[Program.Columns.PROGRAMIDCOLUMN].ToString());
                    program.Name = row[Program.Columns.PROGRAMNAMECOLUMN].ToString();

                    if (dsReportFilters.Tables["ReportDate"].Rows.Count > 0)
                    {
                        program.ReportDate = Convert.ToDateTime(dsReportFilters.Tables["ReportDate"].Rows[0][Program.Columns.REPORTDATE].ToString());
                    }

                    programs.Add(program);
                    

                    if (dsReportFilters.Tables["Groups"].Rows.Count > 0)
                    {
                        program.Groups = new List<Group>();
                        foreach (DataRow groupRow in dsReportFilters.Tables["Groups"].Select(string.Format("ProgramId = '{0}'", program.ProgramId)))
                        {
                            
                            Group group = Group.Build(groupRow);
                            
                            if (dsReportFilters.Tables["Subscribers"].Rows.Count > 0)
                            {
                                group.Subscribers = new List<Subscriber>();
                                foreach (DataRow subscriberRow in dsReportFilters.Tables["Subscribers"].Select(string.Format("ProgramId = '{0}' AND GroupId = '{1}'", program.ProgramId, group.GroupId)))
                                {
                                    Subscriber subscriber = Subscriber.Build(subscriberRow);
                                    
                                    group.Subscribers.Add(subscriber);
                                }
                            }
                            program.Groups.Add(group);
                            
                        }
                    }

                    

                    if (dsReportFilters.Tables["Conditions"].Rows.Count > 0)
                    {
                        program.Conditions = new List<Condition>();
                        foreach (DataRow conditionRow in dsReportFilters.Tables["Conditions"].Select(string.Format("ProgramId = '{0}'", program.ProgramId )))
                        {
                            Condition condition = Condition.Build(conditionRow);
                                                        
                            program.Conditions.Add(condition);

                            if (dsReportFilters.Tables["Populations"].Rows.Count > 0)
                            {
                                condition.Populations = new List<Population>();
                                foreach (DataRow populationRow in dsReportFilters.Tables["Populations"].Select(string.Format("ProgramConditionId = '{0}'", condition.ProgramConditionId)))
                                {
                                    Population population = Population.Build(populationRow);
                                    condition.Populations.Add(population);

                                    if (dsReportFilters.Tables["Measures"].Rows.Count > 0)
                                    {
                                        population.Measures = new List<Measure>();
                                        foreach (DataRow measureRow in dsReportFilters.Tables["Measures"].Select(string.Format("DenominatorId = '{0}'", population.DenominatorId)))
                                        {
                                            Measure measure = Measure.BuildLight(measureRow);
                                            population.Measures.Add(measure);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (dsReportFilters.Tables["Payers"].Rows.Count > 0)
                    {
                        program.Payers = new List<Payor>();
                        foreach (DataRow payerRow in dsReportFilters.Tables["Payers"].Rows)
                        {
                            Payor payer = Payor.Build(payerRow);
                            program.Payers.Add(payer);

                        }
                    }

                    if (dsReportFilters.Tables["AttributionTypes"].Rows.Count > 0)
                    {
                        program.AttributionTypes = new List<AttributionType>();
                        foreach (DataRow attributionTypeRow in dsReportFilters.Tables["AttributionTypes"].Rows)
                        {
                            AttributionType attributionTypes = AttributionType.Build(attributionTypeRow);
                            program.AttributionTypes.Add(attributionTypes);
                        }
                    }
                }
            }
            return programs;
        }
    }
}
