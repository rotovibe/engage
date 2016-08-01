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
        StartsWith = 0,
        ExactMatch = 1,
        Contains = 2
    }

    public enum CareTeamMemberStatus
    {
        Active = 1,
        Inactive = 2,
        Invalid = 3
    }
}
