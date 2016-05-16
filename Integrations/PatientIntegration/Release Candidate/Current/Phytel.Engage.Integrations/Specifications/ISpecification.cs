namespace Phytel.Engage.Integrations.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);
    }
}