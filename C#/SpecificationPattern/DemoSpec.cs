using Braspag.CommonTypes.Enums;
using Pagador.Gateway.Domain;

namespace Pagador.Gateway.Transactional.Engine
{
    public class DemoSpec
    {
        public bool IsCaptured()
        {
            var a = new Transaction
            {
                Status = TransactionStatusEnum.PaymentConfirmed
            };

            return SpecificationImplement.CapturedTransaction.IsSatisfiedBy(a);
        }
    }
}