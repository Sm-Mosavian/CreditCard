using CreditCard.Api.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace CreditCard.Api
{
    [Route("/error")]
    public sealed class ErrorController : BaseApiController
    {
        public IActionResult Error()
        {
            List<ErrorOr.Error> errors = new List<ErrorOr.Error>();
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            errors.Add(ErrorOr.Error.Unexpected(code: "ERRUNEXPECTED", description: exception.Message ?? "An Unexpected error occured"));

            return Problem(errors);
        }
    }
}
