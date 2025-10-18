using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MeetCode.Application.Commands.CommandValidators.Auth;

namespace MeetCode.Tests.Validator
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

        [Fact]
        public async void ShouldHaveError_WhenDisplayNameEmpty()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                "",
                PASSWORD_DUMMY);

            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.DisplayName)
                .WithErrorMessage("Display name is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenDisplayNameOverMaximumLength()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                new string('n', 101),
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.DisplayName)
                .WithErrorMessage("Display name must not greater than 100 characters.");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordIsEmpty()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                DISPLAY_NAME_DUMMY,
                ""
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLessThanMinimumLength()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                DISPLAY_NAME_DUMMY,
                "abc123"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must have more than 8 characters.");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLacksLowercase()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                DISPLAY_NAME_DUMMY,
                "HOANYU12345"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain at least one lowercase letter.");
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLacksNumber()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                DISPLAY_NAME_DUMMY,
                "hoanyuabcde"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain at least one number.");
        }

        [Fact]
        public async void ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var model = new RegisterUserCommand(
                EMAIL_DUMMY,
                DISPLAY_NAME_DUMMY,
                "hoanyu123"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
