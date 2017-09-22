using Braspag.CommonTypes.Enums;
using Pagador.Gateway.Domain;

namespace Pagador.Gateway.Transactional.Engine
{
    public class SpecificationImplement
    {
        public static Specification<Transaction> PaymentConfirmed => Spec.For<Transaction>(
            transaction => transaction.Status == TransactionStatusEnum.PaymentConfirmed);
    }
}