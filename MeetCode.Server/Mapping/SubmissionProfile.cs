using AutoMapper;
using MeetCode.Application.DTOs.Response.Language;
using MeetCode.Application.DTOs.Response.Problem;
using MeetCode.Application.DTOs.Response.Submit;
using MeetCode.Domain.Entities;

namespace MeetCode.Server.Mapping
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submission, SubmissionResponse>()
                .ConstructUsing((src, context) => new SubmissionResponse(
                    src.SubmissionId,
                    src.UserId,
                    context.Mapper.Map<LanguageResponse>(src.Lang),
                    context.Mapper.Map<ProblemResponse>(src.Problem),
                    src.Verdict,
                    src.ExecTimeMs ?? 0
                    ));
        }
    }
}
