using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using MeetCode.Domain.Entities;
using MeetCode.Application.DTOs.Request.Problem;
using MeetCode.Application.DTOs.Response.Problem;
using MeetCode.Application.DTOs.Response.Tag;
using MeetCode.Application.DTOs.Response.TestCase;

namespace MeetCode.Server.Mapping
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            CreateMap<Problem, ProblemResponse>()
                .ConstructUsing((src, context) => new ProblemResponse(
                    src.ProblemId,
                    src.Title,
                    src.Slug,
                    src.StatementMd,
                    src.Difficulty,
                    src.TotalSubmissionCount,
                    src.ScoreAcceptedCount,
                    src.AcceptanceRate,
                    src.Tags.Select(t => new TagResponse(t.TagId, t.Name)).ToList(),
                    context.Mapper.Map<List<TestCaseResponse>>(src.TestCases)
                ));

            CreateMap<Problem, AdminProblemResponse>()
                .ConstructUsing((src, context) => new AdminProblemResponse(
                    src.ProblemId,
                    src.Title,
                    src.Slug,
                    src.StatementMd,
                    src.Difficulty,
                    src.TotalSubmissionCount,
                    src.ScoreAcceptedCount,
                    src.AcceptanceRate,
                    context.Mapper.Map<List<TagResponse>>(src.Tags),
                    context.Mapper.Map<List<TestCaseResponse>>(src.TestCases),
                    src.IsActive,
                    src.CreatedBy
                ));
            // Add
            CreateMap<ProblemAddRequest, ProblemAddCommand>();
            CreateMap<ProblemAddCommandResult, AdminProblemResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<AdminProblemResponse>(src.Problem));

            // Get all
            CreateMap<ProblemAllQueryResult, ProblemAllResponse>()
                .ConstructUsing((src, context) => new ProblemAllResponse(
                    context.Mapper.Map<List<ProblemResponse>>(src.ProblemList)
                    ));
            CreateMap<ProblemAllQueryResult, AdminProblemAllResponse>()
                .ConstructUsing((src, context) => new AdminProblemAllResponse(
                    context.Mapper.Map<List<AdminProblemResponse>>(src.ProblemList)
                    ));

            // Read 
            CreateMap<ProblemReadQueryResult, IProblemResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<IProblemResponse>(src.Problem));

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
