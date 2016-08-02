using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class ObservationValueData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public PreviousValueData PreviousValue { get; set; }
    }
}
