using System;
using System.Threading.Tasks;
using Api.Config;
using Api.Constants;
using Api.Controllers;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Api.Test.Controllers
{
    public class Tests
    {
        private AuthController _authController;

        private Mock<IAuthenticationService> _authService;

        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthenticationService>();
            _authController = new AuthController(_authService.Object);
        }

        [Test]
        public async Task Authenticate_Invokes_Auth_Service_With_Correct_Parameters()
        {
            _authService
                .Setup(service => service.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult {AuthStatus = AuthStatus.Success});

            var request = new AuthRequest {Username = "foo", Password = "bar"};

            await _authController.Authenticate(request);

            _authService.Verify(
                service => service.Authenticate(request.Username, request.Password)
                , Times.Exactly(1)
            );
        }

        [Test]
        public async Task Authenticate_Returns_Proper_Status_Code_On_Auth_Success()
        {
            _authService
                .Setup(service => service.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult {AuthStatus = AuthStatus.Success});

            var request = new AuthRequest {Username = "foo", Password = "bar"};

            var result = await _authController.Authenticate(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Authenticate_Returns_Proper_Status_Code_On_Auth_Failure()
        {
            _authService
                .Setup(service => service.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResult {AuthStatus = AuthStatus.Failure});

            var request = new AuthRequest {Username = "foo", Password = "bar"};

            var result = await _authController.Authenticate(request);

            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

        [Test]
        public void Refresh_Throws_UnsupportedException()
        {
            Assert.Throws<NotImplementedException>(() => _authController.Refresh());
        }
    }
}