using AppDomain.Engage.Population.DTO.Context;

namespace AppDomain.Engage.Population
{
    public interface IDemographicsManager
    {
        UserContext UserContext { get; set; }
        string DoSomething();
    }
}