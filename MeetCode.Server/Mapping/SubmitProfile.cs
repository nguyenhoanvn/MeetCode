using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Job;
using MeetCode.Application.DTOs.Request.Submit;

namespace MeetCode.Server.Mapping
{
    public class SubmitProfile : Profile
    {
        public SubmitProfile()
        {
            // Run code enqueuing
            CreateMap<RunCodeRequest, RunCodeJobCommand>();
        }
    }
}
