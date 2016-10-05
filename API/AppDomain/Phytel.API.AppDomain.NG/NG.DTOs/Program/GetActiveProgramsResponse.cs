using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetActiveProgramsResponse : IDomainResponse
    {
        public List<ProgramInfo> Programs { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ProgramInfo
    {
        public string Id { get; set; }
        public string ProgramSourceId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }
        public int ProgramState { get; set; }
        public int ElementState { get; set; }
        public DateTime? AttrEndDate { get; set; }

        /// <summary>
        /// Assigned patient id to the program
        /// </summary>
        public string PatientId { get; set; }

        public DateTime? StateUpdatedOn { get; set; }

        public DateTime? AssignDate { get; set; }
    }
}
