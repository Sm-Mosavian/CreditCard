using ErrorOr;
using MediatR;

namespace CreditCard.Core.Application
{
    public class ValidateCreditCardRequest : IRequest<ErrorOr<bool>>
    {
        public string CardNumber { get; }

        public ValidateCreditCardRequest(string cardNumber)
        {
            CardNumber = cardNumber;
        }
    }
}
