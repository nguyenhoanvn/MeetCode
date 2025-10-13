using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using ReactASP.Application.Commands.CommandValidators;
using ReactASP.Application.Commands.LoginUser;

namespace ReactASP.Tests.Validator
{
    public class ProblemAddCommandValidatorTests
    {
        private readonly string TITLE_DUMMY = "two sum";
        private readonly string STATEMENT_MD_DUMMY = "This problem need you to add two number up to a target";
        private readonly string DIFFICULTY_DUMMY = "easy";
        private readonly int TIME_LIMIT_DUMMY = 10;
        private readonly int MEMORY_LIMIT_DUMMY = 20;
        private readonly ProblemAddCommandValidator _validator;
        public ProblemAddCommandValidatorTests()
        {
            _validator = new ProblemAddCommandValidator();
        }

        [Fact]
        public async void ShouldHaveError_WhenTitleIsEmpty()
        {
            var model = new ProblemAddCommand(
                "",
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Problem title is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenTitleOverMaximumLength()
        {
            var model = new ProblemAddCommand(
                new string('a', 256),
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Problem title must less than 255 characters.");
        }

        [Fact]
        public async void ShouldHaveError_WhenTitleContainsNonAlphabeticalCharacters()
        {
            var model = new ProblemAddCommand(
                "123InvalidTitle",
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Problem must contain only alphabetical characters.");
        }

        [Fact]
        public async void ShouldHaveError_WhenDifficultyIsEmpty()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                "",
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Difficulty)
                .WithErrorMessage("Problem difficulty is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenDifficultyIsInvalid()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                "invalid",
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Difficulty)
                .WithErrorMessage("Problem difficulty invalid.");
        }

        [Fact]
        public async void ShouldHaveError_WhenTimeLimitIsEmpty()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                0,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.TimeLimitMs)
                .WithErrorMessage("Problem time limit is required.");
        }

        [Fact]
        public async void ShouldHaveError_WhenTimeLimitIsInvalid()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                -1,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.TimeLimitMs)
                .WithErrorMessage("Problem time limit invalid.");
        }

        [Fact]
        public async void ShouldHaveError_WhenMemoryLimitIsEmpty()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                TIME_LIMIT_DUMMY,
                -1
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.MemoryLimitMb)
                .WithErrorMessage("Problem memory is invalid");
        }

        [Fact]
        public async void ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var model = new ProblemAddCommand(
                TITLE_DUMMY,
                STATEMENT_MD_DUMMY,
                DIFFICULTY_DUMMY,
                TIME_LIMIT_DUMMY,
                MEMORY_LIMIT_DUMMY
            );
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
