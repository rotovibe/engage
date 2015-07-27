using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    public class PredicateFilter
    {
        public string FilterKey { get; set; }

        public FilterBooleanType PrefixBoolean { get; set; }

        public TypeCode DataType { get; set; }

        public string FilterField { get; set; }

        public FilterArgumentType? FilterArgument { get; set; }

        public List<FilterValue> FilterValues { get; set; }
    }
}
