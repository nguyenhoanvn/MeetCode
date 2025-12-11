using AutoMapper;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.DTOs.Response.TestCase;

namespace MeetCode.Server.Mapping
{
    public class TestResultProfile : Profile
    {
        public TestResultProfile()
        {
            CreateMap<TestResult, TestResultResponse>()
                .ConstructUsing((src, context) => new TestResultResponse(
                    context.Mapper.Map<TestCaseResponse>(src.TestCase),
                    src.Result,
                    src.IsSuccessful,
                    src.ExecTimeMs
                ));
        }
    }
}
