using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using MeetCode.Domain.Entities;
using MeetCode.Server.DTOs.Request.Problem;
using MeetCode.Server.DTOs.Response.Problem;
using MeetCode.Server.DTOs.Response.Tag;
using MeetCode.Server.DTOs.Response.TestCase;

namespace MeetCode.Server.Mapping
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            CreateMap<Problem, ProblemResponse>()
                .ConstructUsing(src => new ProblemResponse(
                    src.ProblemId,
                    src.Title,
                    src.Slug,
                    src.StatementMd,
                    src.Difficulty,
                    src.TotalSubmissionCount,
                    src.ScoreAcceptedCount,
                    src.AcceptanceRate,
                    src.Tags.Select(t => new TagResponse(t.TagId, t.Name)).ToList(),
                    src.TestCases.Select(tc => new TestCaseResponse(tc.TestId, tc.Visibility, tc.InputText, tc.ExpectedOutputText, tc.Weight, tc.ProblemId)).ToList()
                ));
            // Add
            CreateMap<ProblemAddRequest, ProblemAddCommand>();
            CreateMap<ProblemAddCommandResult, ProblemResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<ProblemResponse>(src.Problem));

            // Get all
            CreateMap<ProblemAllQueryResult, ProblemAllResponse>()
                .ConstructUsing((src, context) => new ProblemAllResponse(
                    context.Mapper.Map<List<ProblemResponse>>(src.ProblemList)
                    ));

            // Read 
            CreateMap<ProblemReadQueryResult, ProblemResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<ProblemResponse>(src.Problem));

            // Update
            CreateMap<(Guid problemId, ProblemUpdateRequest request), ProblemUpdateCommand>()
                .ConstructUsing(src => new ProblemUpdateCommand(
                    src.problemId,
                    src.request.NewStatementMd,
                    src.request.NewDifficulty,
                    src.request.TagIds));
            CreateMap<ProblemUpdateCommandResult, ProblemResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<ProblemResponse>(src.UpdatedProblem));

            // Delete
            CreateMap<(Guid problemId, ProblemDeleteRequest request), ProblemDeleteCommand>()
                .ConstructUsing(src => new ProblemDeleteCommand(
                    src.problemId));
            CreateMap<ProblemDeleteCommandResult, ProblemMessageResponse>();
        }
    }
}
