using AutoMapper;
using ReactASP.Application.Commands.CommandEntities;
using ReactASP.Application.Commands.CommandResults;
using ReactASP.Server.DTOs.Request;
using ReactASP.Server.DTOs.Response;

namespace ReactASP.Server.Mapping
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            // Problem add
            CreateMap<ProblemAddRequest, ProblemAddCommand>();
            CreateMap<ProblemAddResult, ProblemAddResponse>();
        }
    }
}
