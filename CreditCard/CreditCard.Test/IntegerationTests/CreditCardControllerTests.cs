using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CreditCard.Test.IntegerationTests
{
    public class CreditCardControllerTests
    {
        private readonly HttpClient _client;

        public CreditCardControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("4111111111111111", true)]
        [InlineData("5500000000000004", true)]
        [InlineData("1234567812345670", true)]
        public async Task GetValidationResult_ShouldReturnExpectedResult(string cardNumber, bool expected)
        {
            var response = await _client.GetAsync($"/api/v1.0/CreditCard/{cardNumber}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseString);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123456781234567", false)] // Invalid number
        [InlineData("abc123456789012", false)] // Invalid input (letters)
        [InlineData("4111 1111 1111 111a", false)] // Invalid input (mixed characters)
        [InlineData("!@#$%^&*()", false)] // Invalid input (special characters)
        public async Task GetValidationResult_ShouldReturnInvalid_WhenCreditCardIsInvalid(string cardNumber, bool expected)
        {
            var response = await _client.GetAsync($"/api/v1.0/CreditCard/{cardNumber}");

            var actual = response.IsSuccessStatusCode;

            Assert.Equal(expected, actual);

        }

        [Theory]
        [InlineData("abc123456789012")] // Invalid input (letters)
        [InlineData("4111 1111 1111 111a")] // Invalid input (mixed characters)
        [InlineData("!@#$%^&*()")] // Invalid input (special characters)
        public async Task GetValidationResult_ShouldReturnInvalid_WhenCreditCardHasInvalidCharacters(string cardNumber)
        {
            var response = await _client.GetAsync($"/api/v1.0/CreditCard/{cardNumber}");

            var actual = response.IsSuccessStatusCode;

            Assert.False(actual);
        }

        [Fact]
        public async Task GetValidationResult_ShouldReturnBadRequest_WhenCreditCardNumberIsEmpty()
        {
            var response = await _client.GetAsync("/api/v1.0/CreditCard/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
