using ErrorOr;
using MediatR;

namespace CreditCard.Core.Application
{
    public class ValidateCreditCardRequestHandler : IRequestHandler<ValidateCreditCardRequest, ErrorOr<bool>>
    {

        public async Task<ErrorOr<bool>> Handle(ValidateCreditCardRequest request, CancellationToken cancellationToken)
        {
            var result = BeValidLuhn(request.CardNumber);
            return await Task.FromResult(result);
        }
        private bool BeValidLuhn(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(cardNumber[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }
    }
}
