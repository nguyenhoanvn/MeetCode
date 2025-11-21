using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using MeetCode.Application.Commands.CommandEntities;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Server.DTOs.Response.Auth;
using MeetCode.Server.DTOs.Request.Auth;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using MeetCode.Server.Controllers;

namespace MeetCode.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly string EMAIL_DUMMY = "hoanyu12345@gmail.com";
        private readonly string DISPLAY_NAME_DUMMY = "nguyenhoanvn";
        private readonly string PASSWORD_DUMMY = "hoanyu12345";

        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controllerMock;

        public AuthControllerTests()
        {
            _mediatorMock = new();
            _mapperMock = new();
            _loggerMock = new();
            _controllerMock = new(_mediatorMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        public class RegisterTests : AuthControllerTests
        {
            [Fact]
            public async Task ReturnSuccess_WhenRegisterSuccessful()
            {
                var request = new RegisterRequest
                (
                    Email: EMAIL_DUMMY,
                    DisplayName: DISPLAY_NAME_DUMMY,
                    Password: PASSWORD_DUMMY
                );

                var command = new RegisterUserCommand(
                    request.Email,
                    request.DisplayName,
                    request.Password
                    );

                var result = Result<RegisterUserResult>.Success(new RegisterUserResult
                (
                    Guid.NewGuid(),
                    EMAIL_DUMMY,
                    DISPLAY_NAME_DUMMY,
                    "user"
                ));

                var expectedResp = new RegisterResponse(
                    UserId: result.Value.UserId,
                    Email: result.Value.Email,
                    DisplayName: result.Value.DisplayName,
                    Role: result.Value.Role
                    );

                _mapperMock.Setup(m => m.Map<RegisterUserCommand>(It.IsAny<RegisterRequest>())).Returns(command);
                _mapperMock.Setup(m => m.Map<RegisterResponse>(It.IsAny<RegisterUserResult>())).Returns(expectedResp);
                _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

                var resp = await _controllerMock.Register(request, CancellationToken.None);

                Assert.NotNull(resp);
                Assert.Equal(ResultStatus.Ok, resp.Status);
                Assert.NotNull(resp.Value);
                Assert.Equal(expectedResp.Email, resp.Value.Email);
            }
        }
    }
}
