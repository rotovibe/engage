namespace Phytel.Engage.Integrations.Specifications
{
    public interface IParseToDosSpecification<T>
    {
        bool IsSatisfiedBy(T obj);
    }
}