using AutoMapper;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.DTOs.Response.Submit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Worker.Mapping
{
    public class RunCodeProfile : Profile
    {
        public RunCodeProfile()
        {
            CreateMap<RunCodeCommandResult, RunCodeResponse>()
                .ConstructUsing((src, context) => new RunCodeResponse(
                        src.JobId,
                        src.Status,
                        context.Mapper.Map<List<TestResultResponse>>(src.TestResults)
                    ));
        }
    }
}
