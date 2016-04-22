
namespace Phytel.API.DataDomain.Medication.DTO
{
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        Refused = 3,
        NotDoneMedical = 4, 
        Unknown = 5,
        Invalid = 6,
        Duplicate = 7
    }

    public enum Category
    {
        Medication = 1,
        Supplement = 2
    }  
}
