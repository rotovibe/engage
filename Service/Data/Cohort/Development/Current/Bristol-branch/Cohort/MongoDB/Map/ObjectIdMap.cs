using AutoMapper;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Cohort
{
    public class ObjectIdMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<string, ObjectId>().ConvertUsing<ObjectIdTypeConverter>();
        }
    }
}
