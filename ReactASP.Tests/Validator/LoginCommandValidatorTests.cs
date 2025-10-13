using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using ReactASP.Application.Commands.CommandValidators.Auth;
using ReactASP.Application.Commands.RegisterUser;

namespace ReactASP.Tests.Validator
{
    public class LoginCommandValidatorTests
    {
        private readonly string EMAIL_DUMMY = "hoanyu12345@gmail.com";
        private readonly string PASSWORD_DUMMY = "hoanyu123";
        private readonly LoginUserCommandValidator _validator;
        public LoginCommandValidatorTests()
        {
            _validator = new LoginUserCommandValidator();
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new LoginUserCommand(
                "",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required");
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailNotInFormat()
        {
            var model = new LoginUserCommand(
                "hoanyu123",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must have less than 255 characters");
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailOverMaximumLength()
        {
            var model = new LoginUserCommand(
                new string('m', 1000) + "@gmail.com",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must have less than 255 characters");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordIsEmpty()
        {
            var model = new LoginUserCommand(
                EMAIL_DUMMY,
                ""
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is required");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLessThanMinimumLength()
        {
            var model = new LoginUserCommand(
                EMAIL_DUMMY,
                "abc123"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must have more than 8 characters.");
        }

        [Fact]
        public async void ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var model = new LoginUserCommand(
                EMAIL_DUMMY,
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
