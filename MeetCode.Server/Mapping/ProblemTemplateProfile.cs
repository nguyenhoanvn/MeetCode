using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using MeetCode.Application.DTOs.Request.ProblemTemplate;
using MeetCode.Application.DTOs.Response.ProblemTemplate;
using MeetCode.Domain.Entities;

namespace MeetCode.Server.Mapping
{
    public class ProblemTemplateProfile : Profile
    {
        public ProblemTemplateProfile()
        {
            CreateMap<ProblemTemplate, ProblemTemplateResponse>()
                .ConstructUsing(src => new ProblemTemplateResponse(
                    src.TemplateCode));

            CreateMap<(Guid problemId, ProblemTemplateAddRequest request), ProblemTemplateAddCommand>()
                .ConstructUsing(src => new ProblemTemplateAddCommand(
                    src.request.MethodName,
                    src.request.ReturnType,
                    src.request.Parameters.Select(p => p.ToString()).ToArray(),
                    src.problemId,
                    src.request.LangId
                    ));
            CreateMap<ProblemTemplateAddResult, ProblemTemplateResponse>()
                .ConstructUsing((src, context) => context.Mapper.Map<ProblemTemplateResponse>(src.ProblemTemplate));
        }
    }
}
