using AutoMapper;
using ReactASP.Application.Commands.ProblemAdd;
using ReactASP.Server.DTOs.ProblemAdd;

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
