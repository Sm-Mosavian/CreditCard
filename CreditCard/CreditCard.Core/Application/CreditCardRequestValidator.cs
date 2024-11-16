using FluentValidation;

namespace CreditCard.Core.Application
{
    public class CreditCardRequestValidator : AbstractValidator<ValidateCreditCardRequest>
    {
        public CreditCardRequestValidator()
        {
            RuleFor(x => x.CardNumber)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(16).WithMessage("Card number must be exactly 16 digits.")
                .Matches("^[0-9]+$").WithMessage("Card number must consist of digits only.");

        }
    }
}
