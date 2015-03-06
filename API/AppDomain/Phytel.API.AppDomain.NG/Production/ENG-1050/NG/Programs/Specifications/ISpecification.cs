namespace Phytel.API.AppDomain.NG.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);
        //AndSpecification<T> And(Specification<T> specification);
        //OrSpecification<T> Or(Specification<T> specification);
        //NotSpecification<T> Not(Specification<T> specification);
    }
}