using System;
namespace Phytel.API.AppDomain.NG.DTO
{
    public interface IProcessActionRequest
    {
        Actions Action { get; set; }
        string ContractNumber { get; set; }
        string PatientId { get; set; }
        string ProgramId { get; set; }
        string Token { get; set; }
        string UserId { get; set; }
        double Version { get; set; }
    }
}
