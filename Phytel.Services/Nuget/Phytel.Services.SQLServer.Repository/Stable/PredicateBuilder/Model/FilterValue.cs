using System;
using System.Runtime.Serialization;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    public class FilterValue
    {
        public string Value { get; set; }

        public FilterValue() { }

        public FilterValue(string value)
        {
            Value = value;
        }
    }
}
