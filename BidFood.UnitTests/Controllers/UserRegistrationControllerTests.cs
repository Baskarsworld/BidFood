using Bidfood.Controllers;
using Bidfood.Infrastructure;
using Bidfood.Models;
using Bidfood.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace BidFood.UnitTests.Services
{
    public class UserRegistrationControllerTests
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly UserRegistrationController _controller;

        public UserRegistrationControllerTests()
        {
            _userRegistrationService = Substitute.For<IUserRegistrationService>();
            _controller = new UserRegistrationController(_userRegistrationService);
        }

        [Fact]
        public void User_Should_Be_Registered_Successfully_For_Valid_Request()
        {
            //Arrange
            var userDetail = new UserDetail
            {
                FirstName = "firstName",
                LastName = "lastName",
            };
            _userRegistrationService.UserRegistration(Arg.Any<UserDetail>())
                .Returns(new ApiResponse
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                });

            //Act
            var response = _controller.Post(userDetail);
            var result = response as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
        }

    }
}
