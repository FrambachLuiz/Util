using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pagador.Gateway.Transactional.Engine
{
    internal class GenericSpecification<T> : Specification<T>
    {
        internal static readonly Specification<T> All = new GenericSpecification<T>(x => true);

        internal static readonly Specification<T> None = new GenericSpecification<T>(x => false);

        private Func<T, bool> _compiledFunc;

        public GenericSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }

        public override Expression<Func<T, bool>> Expression { get; }

        public override bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc = _compiledFunc ?? Expression.Compile();
            return _compiledFunc(candidate);
        }
    }
}
