using System;
namespace Phytel.API.AppDomain.NG.DTO
{
    public interface IPlanElement
    {
        string Id { get; set; }
        bool Completed { get; set; }
        bool Enabled { get; set; }
        string Next { get; set; }
        int Order { get; set; }
        string Previous { get; set; }
    }
}
