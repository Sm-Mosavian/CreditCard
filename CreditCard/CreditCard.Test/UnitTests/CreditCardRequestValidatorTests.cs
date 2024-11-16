using CreditCard.Core.Application;
using FluentValidation.TestHelper;


namespace CreditCard.Test.UnitTests
{
    public class CreditCardRequestValidatorTests
    {
        private readonly CreditCardRequestValidator _validator;

        public CreditCardRequestValidatorTests()
        {
            _validator = new CreditCardRequestValidator();
        }

        [Fact]
        public void CardNumber_Should_Have_Error_When_Empty()
        {
            // Arrange
            var request = new ValidateCreditCardRequest(string.Empty);

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors,
                error => error.PropertyName == nameof(request.CardNumber) &&
                         error.ErrorMessage == "Card number is required.");
        }

        [Fact]
        public void CardNumber_Should_Have_Error_When_Not_16_Digits()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("123456789012345"); // 15 digits

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors,
                error => error.PropertyName == nameof(request.CardNumber) &&
                         error.ErrorMessage == "Card number must be exactly 16 digits.");
        }

        [Fact]
        public void CardNumber_Should_Have_Error_When_Contains_NonDigit_Characters()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("1234-5678-9012-3456"); // Contains hyphens

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors,
                error => error.PropertyName == nameof(request.CardNumber) &&
                         error.ErrorMessage == "Card number must consist of digits only.");
        }

        [Fact]
        public void CardNumber_Should_Not_Have_Error_When_Valid()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("1234567890123456"); // Valid card number

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}



