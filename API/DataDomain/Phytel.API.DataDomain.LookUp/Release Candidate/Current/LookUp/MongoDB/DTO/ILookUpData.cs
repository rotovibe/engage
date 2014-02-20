using MongoDB.Bson;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public interface ILookUpData
    {
        ObjectId DataID { get; set; }
        string Name { get; set; }
    }
}
