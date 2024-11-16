using CreditCardValidator.Core.Application;
using ErrorOr;

namespace CreditCard.Test.UnitTests
{
    public class ValidateCreditCardRequestHandlerTests
    {
        private readonly ValidateCreditCardRequestHandler _handler;

        public ValidateCreditCardRequestHandlerTests()
        {
            _handler = new ValidateCreditCardRequestHandler();
        }

        [Theory]
        [InlineData("4111111111111111", true)]
        [InlineData("5500000000000004", true)]
        [InlineData("1234567812345670", true)]
        [InlineData("6011000000000004", true)]
        [InlineData("4111111111111120", false)]
        [InlineData("5500000000000005", false)]
        [InlineData("6011000000000003", false)]
        public async Task Handle_Should_Return_Expected_Result(string cardNumber, bool expected)
        {
            // Arrange
            var request = new ValidateCreditCardRequest(cardNumber);

            // Act
            ErrorOr<bool> result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(expected, result.Value);
        }
    }


}
