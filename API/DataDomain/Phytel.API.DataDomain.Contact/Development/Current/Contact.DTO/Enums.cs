namespace Phytel.API.DataDomain.Contact.DTO
{
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        Archived = 3
    }

    public enum Deceased
    {
        None = 0,
        Yes = 1,
        No = 2
    }

    public enum ContactLookUpGroupType
    {
        Unknown = 0,
        ContactType = 1,
        CareTeam = 2

    }

    public enum FilterType
    {
        StartsWith = 1,
        ExactMatch = 2
    }
}
