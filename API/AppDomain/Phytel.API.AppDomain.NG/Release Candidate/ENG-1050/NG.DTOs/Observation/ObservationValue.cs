using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class ObservationValue
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public PreviousValue PreviousValue { get; set; }
    }
}
