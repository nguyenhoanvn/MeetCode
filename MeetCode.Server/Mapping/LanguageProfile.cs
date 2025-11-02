using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.Queries.QueryEntities.Language;
using MeetCode.Application.Queries.QueryResults.Language;
using MeetCode.Domain.Entities;
using MeetCode.Server.DTOs.Request.Language;
using MeetCode.Server.DTOs.Response.Language;

namespace MeetCode.Server.Mapping
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageResponse>()
                .ConstructUsing(src => new LanguageResponse(
                    src.Name,
                    src.Version,
                    src.RuntimeImage,
                    src.CompileCommand,
                    src.RunCommand,
                    src.IsEnabled
                ));

            // Read
            CreateMap<(Guid langID, LanguageReadRequest request), LanguageReadQuery>()
                .ConstructUsing(src => new LanguageReadQuery(
                    src.langID));
            CreateMap<LanguageReadQueryResult, LanguageResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<LanguageResponse>(src.Language)
                    );


            // Update
            CreateMap<(string name, LanguageUpdateRequest request), LanguageUpdateCommand>()
                .ConstructUsing(src => new LanguageUpdateCommand(
                    src.name,
                    src.request.Version,
                    src.request.RuntimeImage,
                    src.request.CompileCommand,
                    src.request.RunCommand
                ));

            CreateMap<LanguageUpdateCommandResult, LanguageResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<LanguageResponse>(src.Language)
                    );
        }
    }
}
