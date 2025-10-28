using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.TestCase;

namespace MeetCode.Application.Queries.QueryValidators.TestCase
{
    public class TestCaseReadQueryValidator : AbstractValidator<TestCaseReadQuery>
    {
        public TestCaseReadQueryValidator()
        {
            RuleFor(x => x.TestId)
                .NotEmpty().WithMessage("Test ID is required.")
                .Must(tc => tc.GetType() == typeof(Guid)).WithMessage("Test ID data type mismatch.");
        }
    }
}
