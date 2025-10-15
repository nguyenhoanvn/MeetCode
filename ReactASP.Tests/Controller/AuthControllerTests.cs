using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MediatR;
using AutoMapper;
using ReactASP.Server.Controllers;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using ReactASP.Application.Commands.LoginUser;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;
using ReactASP.Server.DTOs.Request.Auth;
using ReactASP.Server.DTOs.Response.Auth;

namespace ReactASP.Tests.Controller
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

        [Fact]
        public async Task Register_ReturnCreatedAtAction_WhenRegisterSuccessful()
        {
            var request = new RegisterRequest
            {
                Email = "hoanyu123456@gmail.com",
                DisplayName = "nguyenhoancn",
                Password = "hoanyu1325"
            };

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

            var expectedResp = new RegisterResponse
            {
                UserId = result.Value.UserId,
                DisplayName = result.Value.DisplayName,
                Email = result.Value.Email,
                Role = result.Value.Role
            };

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

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request body", badRequest.Value);
        }
    }
}
