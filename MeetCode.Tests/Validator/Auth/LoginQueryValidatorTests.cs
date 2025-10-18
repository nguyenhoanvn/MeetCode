using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryValidators.Auth;

namespace MeetCode.Tests.Validator.Auth
{
    public class LoginQueryValidatorTests
    {
        private readonly string EMAIL_DUMMY = "hoanyu12345@gmail.com";
        private readonly string PASSWORD_DUMMY = "hoanyu123";
        private readonly LoginUserQueryValidator _validator;
        public LoginQueryValidatorTests()
        {
            _validator = new LoginUserQueryValidator();
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new LoginUserQuery(
                "",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailNotInFormat()
        {
            var model = new LoginUserQuery(
                "hoanyu123",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailOverMaximumLength()
        {
            var model = new LoginUserQuery(
                new string('m', 1000) + "@gmail.com",
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordIsEmpty()
        {
            var model = new LoginUserQuery(
                EMAIL_DUMMY,
                ""
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async void ShouldHaveError_WhenPasswordLessThanMinimumLength()
        {
            var model = new LoginUserQuery(
                EMAIL_DUMMY,
                "abc123"
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async void ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var model = new LoginUserQuery(
                EMAIL_DUMMY,
                PASSWORD_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
