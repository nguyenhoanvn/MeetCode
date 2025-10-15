using AutoMapper;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
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
            CreateMap<ProblemAddResult, ProblemAddResponse>();

            // Problem read
            CreateMap<ProblemAllResult, ProblemAllResponse>();

            // Problem 
        }
    }
}
