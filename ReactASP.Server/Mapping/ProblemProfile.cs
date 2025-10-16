using AutoMapper;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Queries.QueryResults.Problem;
using ReactASP.Server.DTOs.Request.Problem;
using ReactASP.Server.DTOs.Response.Problem;

namespace ReactASP.Server.Mapping
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            // Problem add
            CreateMap<ProblemAddRequest, ProblemAddCommand>();
            CreateMap<ProblemAddCommandResult, ProblemAddResponse>();

            // Problem read all
            CreateMap<ProblemAllQueryResult, ProblemAllResponse>();

            // Problem read one
            CreateMap<ProblemReadQueryResult, ProblemReadResponse>()
                .ForCtorParam("Title", opt => opt.MapFrom((src, ctx) => ctx.Items["Title"]))
                .ForCtorParam("StatementMd", opt => opt.MapFrom((src, ctx) => ctx.Items["StatementMd"]))
                .ForCtorParam("Difficulty", opt => opt.MapFrom((src, ctx) => ctx.Items["Difficulty"]))
                .ForCtorParam("TotalSubmissionCount", opt => opt.MapFrom((src, ctx) => ctx.Items["TotalSubmissionCount"]))
                .ForCtorParam("ScoreAcceptedCount", opt => opt.MapFrom((src, ctx) => ctx.Items["ScoreAcceptedCount"]))
                .ForCtorParam("AcceptanceRate", opt => opt.MapFrom((src, ctx) => ctx.Items["AcceptanceRate"]));
        }
    }
}
