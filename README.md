
# CreditCard

## Overview
CreditCard is a C# .NET 8 application designed to validate credit card numbers using the Luhn algorithm. The solution consists of three main projects:
- **CreditCard.Api**: A Web API project handling HTTP requests.
- **CreditCard.Core**: A library containing the application logic, including request validation and business rules.
- **CreditCardValidator.Test**: A project for testing the application, including unit tests, integration tests, and architecture tests.

## Project Structure
- **CreditCard.Api**
  - Controllers
    - CreditCardController.cs: Handles incoming requests to validate credit cards.
    - BaseApiController.cs: Contains base functionality for API controllers.
    - ErrorController.cs: Manages error handling for the API.
  - Program.cs: The entry point for the API, configuring services and middleware.

- **CreditCard.Core**
  - Application
    - CreditCardRequestValidator.cs: Validates credit card requests.
    - ValidateCreditCardRequest.cs: Represents the request for validating a credit card.
    - ValidateCreditCardRequestHandler.cs: Handles the validation request.
    - ValidationBehavior.cs: Middleware for validating requests before processing.
    - ApplicationServicesRegistration.cs: Configures services for dependency injection.

- **CreditCard.Test**
  - UnitTest: Contains unit tests for the application logic.
    - LuhnValidatorTests.cs: Tests for the credit card validation logic.
  - IntegrationTest: Tests for the API endpoints.
    - CreditCardControllerTests.cs: Tests for the CreditCardController.
  

## Data Flow in the CreditCard Solution
- The "CreditCard" solution is structured into three main projects: CreditCard.Api, CreditCard.Core, and CreditCard.Test.
  Each project plays a distinct role in the overall architecture, promoting separation of concerns and enabling efficient testing.

- When a request reaches the CreditCardController in the CreditCardValidator.Api project, it triggers the validation process for a credit card number.
  The controller creates a MediatR request of type ValidateCreditCardRequest, which is then dispatched to the corresponding request handler, ValidateCreditCardRequestHandler,
  defined in the CreditCard.Core project.

- To ensure user input is validated before any business logic is executed, a ValidationBehavior middleware is implemented.
  This behavior intercepts MediatR requests and utilizes the FluentValidation library through the CreditCardRequestValidator class.
  FluentValidation helps define rules for validating the credit card number, such as ensuring it is not empty, is exactly 16 digits long, and consists solely of numeric characters.
  The CascadeMode.Continue option is employed, allowing the validation process to capture all potential exceptions at once, providing comprehensive feedback for the user.

- In case of validation failures, any exceptions encountered are encapsulated using the ErrorOr library. This library enhances the result pattern,
  allowing the API to return structured error responses to the client efficiently. If validation passes, the request proceeds to the handler.

- The ValidateCreditCardRequestHandler is responsible for executing the core business logic. It validates the credit card number against the Luhn algorithm,
  which checks for the validity of the number based on a mathematical formula. The result is returned as an ErrorOr<bool> object, indicating either success (true) or failure (false).

- In the API project, the CreditCardController extends BaseApiController, which is designed to handle exceptions identified from the ErrorOr object appropriately.
  Additionally, an ErrorController is utilized to handle unexpected exceptions. This controller captures exceptions through middleware registered in the Program.cs file,
  providing a centralized way to respond to errors.

- Service registrations necessary for the application are defined in the ApplicationServicesRegistration class, which is invoked in Program.cs of the API project.
  This class configures dependency injection for FluentValidation and MediatR, ensuring that the validation behavior is applied uniformly across requests.

- The CreditCard.Test project contains three distinct folders for unit tests, integration tests, and architecture tests. Each class in the Core project is tested individually to ensure correct functionality.
  For the CreditCardController, integration tests are implemented to verify that the API behaves as expected under various scenarios, including both valid and invalid credit card numbers.

- This structure not only ensures that user input validation is handled in a dedicated class but also separates concerns effectively.
  The design promotes maintainability and testability, resulting in a robust credit card validation solution.
