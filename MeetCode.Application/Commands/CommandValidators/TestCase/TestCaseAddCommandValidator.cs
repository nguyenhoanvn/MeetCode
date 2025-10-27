using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.TestCase;
using MeetCode.Domain.Enums;

namespace MeetCode.Application.Commands.CommandValidators.TestCase
{
    public class TestCaseAddCommandValidator : AbstractValidator<TestCaseAddCommand>
    {
        public TestCaseAddCommandValidator()
        {
            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("Problem ID is required.")
                .Must(x => x.GetType() == typeof(Guid));
            RuleFor(x => x.Visibility)
                .NotEmpty().WithMessage("Visibility is required")
                .Must(TestCaseVisibility.IsValid).WithMessage("Visibility must be either \"sample\", \"public\" or \"hidden\".");
            RuleFor(x => x.InputText)
                .NotEmpty().WithMessage("Input of test case is required.");
            RuleFor(x => x.ExpectedOutputText)
                .NotEmpty().WithMessage("Expected output of test case is required.");
            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Weight score is required.")
                .Must(tc => tc > 0 && tc < int.MaxValue).WithMessage("Test case weight must be a positive number"); ;
        }
    }
}
