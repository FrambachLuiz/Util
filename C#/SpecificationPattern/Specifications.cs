using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pagador.Gateway.Transactional.Engine
{
    /// <summary>
    /// Specification defined by an Expressions that can be used by IQueryables.
    /// Implements !, &amp; and | operators.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Holds the compiled expression so that it doesn't need to compile it everytime.
        /// </summary>
        private Func<T, bool> _compiledFunc;

        public virtual bool IsSatisfiedBy(T entity)
        {
            _compiledFunc = _compiledFunc ?? this.Expression.Compile();
            return _compiledFunc(entity);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }

        public static And<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new And<T>(left, right);
        }

        public static Or<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new Or<T>(left, right);
        }

        public static Specification<T> operator ==(bool value, Specification<T> spec)
        {
            return value ? spec : !spec;
        }

        public static Specification<T> operator ==(Specification<T> spec, bool value)
        {
            return value ? spec : !spec;
        }

        public static Specification<T> operator !=(bool value, Specification<T> spec)
        {
            return value ? !spec : spec;
        }

        public static Specification<T> operator !=(Specification<T> spec, bool value)
        {
            return value ? !spec : spec;
        }

        public static Not<T> operator !(Specification<T> spec)
        {
            return new Not<T>(spec);
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.Expression;
        }

        public static implicit operator Func<T, bool>(Specification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public override string ToString()
        {
            return Expression.ToString();
        }

        public sealed class And<T> : Specification<T>, IAndSpecification<T>
        {
            public Specification<T> Left { get; }

            public Specification<T> Right { get; }

            ISpecification<T> IAndSpecification<T>.Left => Left;

            ISpecification<T> IAndSpecification<T>.Right => Left;

            internal And(Specification<T> left, Specification<T> right)
            {
                Left = left ?? throw new ArgumentNullException("left");
                Right = right ?? throw new ArgumentNullException("right");
            }

            public override Expression<Func<T, bool>> Expression => Left.Expression.And(Right.Expression);

            public new bool IsSatisfiedBy(T candidate)
            {
                return Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Or<T> : Specification<T>, IOrSpecification<T>
        {
            public Specification<T> Left { get; }

            public Specification<T> Right { get; }

            ISpecification<T> IOrSpecification<T>.Left => Left;

            ISpecification<T> IOrSpecification<T>.Right => Left;

            internal Or(Specification<T> left, Specification<T> right)
            {
                Left = left ?? throw new ArgumentNullException("left");
                Right = right ?? throw new ArgumentNullException("right");
            }

            public override Expression<Func<T, bool>> Expression => Left.Expression.Or(Right.Expression);

            public new bool IsSatisfiedBy(T candidate)
            {
                return Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Not<T> : Specification<T>, INotSpecification<T>
        {
            public Specification<T> Inner { get; }

            ISpecification<T> INotSpecification<T>.Inner => Inner;

            internal Not(Specification<T> spec)
            {
                Inner = spec ?? throw new ArgumentNullException("spec");
            }

            public override Expression<Func<T, bool>> Expression => Inner.Expression.Not();

            public new bool IsSatisfiedBy(T candidate)
            {
                return !Inner.IsSatisfiedBy(candidate);
            }
        }
    }
}