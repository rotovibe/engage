namespace Phytel.API.AppDomain.NG
{
    public interface INGCommand
    {
        void Execute();
        void Undo();
    }
}
