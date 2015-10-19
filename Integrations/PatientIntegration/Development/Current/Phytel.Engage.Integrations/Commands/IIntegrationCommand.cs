namespace Phytel.Engage.Integrations.Commands
{
    public interface IIntegrationCommand<TOut, TParameter>
    {
        TOut Execute(TParameter val);
    }
}