using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;


namespace Phytel.API.DataDomain.Cohort
{
    public class ObjectIdTypeConverter : TypeConverter<string, ObjectId>
    {
        protected override ObjectId ConvertCore(string source)
        {
            ObjectId rvalue = ObjectId.Empty;

            bool parsed = ObjectId.TryParse(source, out rvalue);
            if (!parsed)
            {
                rvalue = ObjectId.Empty;
            }

            return rvalue;
        }
    }
}
