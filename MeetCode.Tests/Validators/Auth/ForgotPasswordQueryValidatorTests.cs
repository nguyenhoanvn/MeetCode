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
    public class ForgotPasswordQueryValidatorTests
    {
        private readonly string EMAIL_DUMMY = "hoanyu12345@gmail.com";
        private readonly ForgotPasswordQueryValidator _validator;
        public ForgotPasswordQueryValidatorTests()
        {
            _validator = new ForgotPasswordQueryValidator();
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new ForgotPasswordQuery("");
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async void ShouldHaveError_WhenEmailNotInFormat()
        {
            var model = new ForgotPasswordQuery("hoanyu12345");
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async void ShouldNotHaveError_WhenEmailIsValid()
        {
            var model = new ForgotPasswordQuery(EMAIL_DUMMY);
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
