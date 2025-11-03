using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandValidators.Auth;

namespace MeetCode.Tests.Validator.Auth
{
    public class ResetPasswordCommandValidatorTests
    {
        private readonly string OTP_DUMMY = "012345";
        private readonly string PASSWORD_DUMMY = "hoanyu123";
        private readonly ResetPasswordCommandValidator _validator;      
        public ResetPasswordCommandValidatorTests()
        {
            _validator = new ResetPasswordCommandValidator();
        }
        [Fact]
        public async void ShouldNotHaveError_WhenAllFieldsValid()
        {
            var model = new ResetPasswordCommand(
                OTP_DUMMY,
                PASSWORD_DUMMY
                );

            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async void ShouldHaveError_WhenOTPEmpty()
        {
            var model = new ResetPasswordCommand(
                "",
                PASSWORD_DUMMY
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public async void ShouldHaveError_WhenOTPNotANumber()
        {
            var model = new ResetPasswordCommand(
                "hehe12",
                PASSWORD_DUMMY
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordEmpty()
        {
            var model = new ResetPasswordCommand(
                OTP_DUMMY,
                ""
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLowerThanMinimumLength()
        {
            var model = new ResetPasswordCommand(
                OTP_DUMMY,
                "heasd1"
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordNotContainAlphabetic()
        {
            var model = new ResetPasswordCommand(
                OTP_DUMMY,
                "123456789"
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordNotContainNumeric()
        {
            var model = new ResetPasswordCommand(
                OTP_DUMMY,
                "hoanyuhehehe"
                );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}
