namespace Phytel.API.AppDomain.NG.PatientSystem.Modifiers
{
    public abstract class ModifierBase<TReturn,TParam>
    {
        public abstract TReturn Modify(TParam param);
    }
}
