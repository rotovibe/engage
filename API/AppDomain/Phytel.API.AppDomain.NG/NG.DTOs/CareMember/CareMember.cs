using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class CareMember
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string ContactId { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string TypeId { get; set; }
        public bool Primary { get; set; }
    }
}
