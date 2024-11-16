using CreditCard.Core.Application;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace CreditCard.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CreditCardController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CreditCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{cardNumber}")]
        public async Task<IActionResult> validate(string cardNumber)
        {
            var creditCardCommand = new ValidateCreditCardRequest(cardNumber);

            ErrorOr<bool> errorOrValid = await _mediator.Send(creditCardCommand);
            return errorOrValid.Match(
                success => Ok(success),
                errors => Problem(errors));
        }

    }
}