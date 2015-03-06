using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.Specifications
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        public NotSpecification(Specification<T> leftSide, Specification<T> rightSide)
            : base(leftSide, rightSide)
        {
        }

        public override bool IsSatisfiedBy(T obj)
        {
            return _leftSide.IsSatisfiedBy(obj) && !_rightSide.IsSatisfiedBy(obj);
        }
    }
}
