using MongoDB.Bson;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public interface ILookUpData
    {
        ObjectId DataId { get; set; }
        string Name { get; set; }
    }
}
