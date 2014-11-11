namespace Phytel.Services.Journal
{
    public interface IJournaledAsChildRequest : IJournaledRequest
    {
        string ParentActionId { get; set; }
    }
}