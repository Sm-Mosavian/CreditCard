using CreditCard.Core.Application;
using ErrorOr;
using MediatR;
using Moq;

namespace CreditCard.Test.UnitTests
{
    public class ValidationBehaviorTests
    {
        private readonly Mock<RequestHandlerDelegate<ErrorOr<bool>>> _next;
        private readonly ValidationBehavior<ValidateCreditCardRequest, ErrorOr<bool>> _validationBehavior;
        private readonly CreditCardRequestValidator _validator;

        public ValidationBehaviorTests()
        {
            _next = new Mock<RequestHandlerDelegate<ErrorOr<bool>>>();
            _validator = new CreditCardRequestValidator();
            _validationBehavior = new ValidationBehavior<ValidateCreditCardRequest, ErrorOr<bool>>(_validator);
        }

        [Fact]
        public async Task Handle_Should_Return_ValidationErrors_When_CardNumber_Is_Empty()
        {
            // Arrange
            var request = new ValidateCreditCardRequest(string.Empty);

            // Act
            var result = await _validationBehavior.Handle(request, _next.Object, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(3, result.Errors.Count);
            Assert.Contains(result.Errors, error => error.Code == "CardNumber" && error.Description == "Card number is required.");
            Assert.Contains(result.Errors, error => error.Code == "CardNumber" && error.Description == "Card number must be exactly 16 digits.");
            Assert.Contains(result.Errors, error => error.Code == "CardNumber" && error.Description == "Card number must consist of digits only.");
        }

        [Fact]
        public async Task Handle_Should_Return_ValidationErrors_When_CardNumber_Is_Invalid_Length()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("123");

            // Act
            var result = await _validationBehavior.Handle(request, _next.Object, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Single(result.Errors);
            Assert.Contains(result.Errors, error => error.Code == "CardNumber" && error.Description == "Card number must be exactly 16 digits.");
        }

        [Fact]
        public async Task Handle_Should_Return_ValidationErrors_When_CardNumber_Contains_NonDigits()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("1234-5678-9012-3456");

            // Act
            var result = await _validationBehavior.Handle(request, _next.Object, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Collection(result.Errors,
                error => Assert.Equal("CardNumber", error.Code),
                error => Assert.Equal("Card number must consist of digits only.", error.Description));
        }

        [Fact]
        public async Task Handle_Should_Call_Next_When_CardNumber_Is_Valid()
        {
            // Arrange
            var request = new ValidateCreditCardRequest("1234567812345678");
            _next.Setup(n => n()).ReturnsAsync(true);

            // Act
            var result = await _validationBehavior.Handle(request, _next.Object, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.True(result.Value);
            _next.Verify(n => n(), Times.Once);
        }
    }
}
