using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetProgramResponse : IDomainResponse
    {
        public Program Program { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Program
    {
        public string ProgramID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //public string EligibilityRequirements { get; set; }
        //public string EligibilityStartDate { get; set; }
        //public string EligibilityEndDate { get; set; }
        public string Status { get; set; }
        public string ProgramStatus { get; set; }
        //public List<Objective> Objectives { get; set; }
        //public List<string> Attributes { get; set; }
        public string AuthoredBy { get; set; }
        public bool Locked { get; set; }
        public string Version { get; set; }
    }

    public class Objective
    {

    }
}
