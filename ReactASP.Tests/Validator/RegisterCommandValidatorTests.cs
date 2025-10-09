using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using ReactASP.Application.Commands.RegisterUser;

namespace ReactASP.Tests.Validator
{
    public class RegisterCommandValidatorTests
    {
        private readonly string EMAIL_DUMMY = "hoanyu12345@gmail.com";
        private readonly string DISPLAY_NAME_DUMMY = "nguyenhoanvn";
        private readonly string PASSWORD_DUMMY = "hoanyu123";
        private readonly RegisterUserCommandValidator _validator;
        public RegisterCommandValidatorTests()
        {
            _validator = new RegisterUserCommandValidator();
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new RegisterUserCommand(
                "",
                DISPLAY_NAME_DUMMY,
                PASSWORD_DUMMY
            );

            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailNotInFormat()
        {
            var model = new RegisterUserCommand(
                "hoanyu123",
                DISPLAY_NAME_DUMMY,
                PASSWORD_DUMMY
                );

            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format.");
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailOverMaximumLength()
        {
            var model = new RegisterUserCommand(
                new string('m', 1000) + "@gmail.com",
                DISPLAY_NAME_DUMMY,
                PASSWORD_DUMMY
                );

            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must not greater than 255 characters.");
        }
    }
}
