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
using MeetCode.Server.Controllers;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace MeetCode.Tests.Controller
{
    public class AuthControllerTests
    {
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
            public async Task Register_ReturnCreatedAtAction_WhenRegisterSuccessful()
            {
                var request = new RegisterRequest
                (
                    Email: "hoanyu12345@gmail.com",
                    DisplayName: "nguyenhoancn",
                    Password: "hoanyu1325"
                );

                var command = new RegisterUserCommand(
                    request.Email,
                    request.DisplayName,
                    request.Password
                    );

                var result = Result<RegisterUserResult>.Success(new RegisterUserResult
                (
                    Guid.NewGuid(),
                    "hoanyu12345@gmail.com",
                    "nguyenhoancn",
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

                var createdResult = Assert.IsType<CreatedAtActionResult>(resp);
                Assert.NotNull(createdResult.Value);

                var actualResp = Assert.IsType<RegisterResponse>(createdResult.Value);
                Assert.Equal(expectedResp.Email, actualResp.Email);
            }

            [Fact]
            public async Task Register_ReturnBadRequest_WhenRequestNull()
            {
                RegisterRequest? request = null;

                var result = await _controllerMock.Register(request, CancellationToken.None);

                var badRequest = Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

            }
        }
    }
}
