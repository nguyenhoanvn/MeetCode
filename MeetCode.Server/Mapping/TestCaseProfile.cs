using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.TestCase;
using MeetCode.Application.Commands.CommandResults.TestCase;
using MeetCode.Application.Queries.QueryEntities.TestCase;
using MeetCode.Application.Queries.QueryResults.TestCase;
using MeetCode.Domain.Entities;
using MeetCode.Server.DTOs.Request.TestCase;
using MeetCode.Server.DTOs.Response.TestCase;

namespace MeetCode.Server.Mapping
{
    public class TestCaseProfile : Profile
    {
        public TestCaseProfile()
        {
            CreateMap<TestCase, TestCaseResponse>()
                .ConstructUsing(src => new TestCaseResponse(
                    src.TestId,
                    src.Visibility,
                    src.InputText,
                    src.ExpectedOutputText,
                    src.Weight,
                    src.ProblemId
                ));

            // Add
            CreateMap<(Guid problemId, TestCaseAddRequest request), TestCaseAddCommand>()
                .ConstructUsing(src => new TestCaseAddCommand(
                    src.problemId,
                    src.request.Visibility,
                    src.request.InputText,
                    src.request.OutputText,
                    src.request.Weight
                ));
            CreateMap<TestCaseAddCommandResult, TestCaseResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<TestCaseResponse>(src.TestCase));

            // Get all
            CreateMap<TestCaseAllRequest, TestCaseAllQuery>();
            CreateMap<TestCaseAllQueryResult, TestCaseAllResponse>()
                .ConstructUsing((src, context) => new TestCaseAllResponse(
                        context.Mapper.Map<IEnumerable<TestCaseResponse>>(src.TestCaseList)
                    ));

            // Get
            CreateMap<(Guid testId, TestCaseReadRequest request), TestCaseReadQuery>()
                .ConstructUsing(src => new TestCaseReadQuery(
                        src.testId
                    ));
            CreateMap<TestCaseReadQueryResult, TestCaseResponse>()
                .ConstructUsing((src, context) =>
                        context.Mapper.Map<TestCaseResponse>(src.TestCase)
                    );

            // Update
            CreateMap<(Guid testId, TestCaseUpdateRequest request), TestCaseUpdateCommand>()
                .ConstructUsing(src => new TestCaseUpdateCommand(
                        src.testId,
                        src.request.Visibility,
                        src.request.InputText,
                        src.request.ExpectedOutputText,
                        src.request.Weight
                    ));
            CreateMap<TestCaseUpdateCommandResult, TestCaseResponse>()
                .ConstructUsing((src, context) =>
                        context.Mapper.Map<TestCaseResponse>(src.TestCase)
                    );

            // Delete
            CreateMap<(Guid testId, TestCaseDeleteRequest request), TestCaseDeleteCommand>()
                .ConstructUsing(src => new TestCaseDeleteCommand(
                        src.testId
                    ));
            CreateMap<TestCaseDeleteCommandResult, TestCaseMessageResponse>();
        }
    }
}
