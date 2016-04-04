using AppDomain.Engage.Clinical.DTO.Context;

namespace AppDomain.Engage.Clinical
{
    public interface IClinicalManager
    {
        UserContext UserContext { get; set; }
    }
}
