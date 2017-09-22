using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pagador.Gateway.Transactional.Engine
{
    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification
    {
        bool IsSatisfiedBy(T candidate);

        Expression<Func<T, bool>> Expression { get; }
    }

    public interface IAndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Left { get; }

        ISpecification<T> Right { get; }
    }

    public interface IOrSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Left { get; }

        ISpecification<T> Right { get; }
    }

    public interface INotSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Inner { get; }
    }
}
