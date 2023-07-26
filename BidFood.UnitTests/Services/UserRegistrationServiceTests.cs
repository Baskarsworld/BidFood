using Bidfood.Common;
using Bidfood.Infrastructure;
using Bidfood.Models;
using Bidfood.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using Xunit;

namespace BidFood.UnitTests.Services
{
    public class UserRegistrationServiceTests
    {
        private readonly ILogger<UserRegistrationService> _logger;
        private readonly IFileProcessService _fileProcessService;
        private readonly IApiResponseFactory _apiResponseFactory;
        private readonly UserRegistrationService _service;

        public UserRegistrationServiceTests()
        {
            _logger = Substitute.For<ILogger<UserRegistrationService>>();
            _fileProcessService = Substitute.For<IFileProcessService>();
            _apiResponseFactory = new ApiResponseFactory();
            _service = new UserRegistrationService(_logger, _fileProcessService, _apiResponseFactory);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("test", null)]
        [InlineData(null, "test")]
        [InlineData("test", "")]
        [InlineData("", "test")]
        public void Should_Return_BadRequest_When_Request_Is_Invalid(string firstName, string lastName)
        {
            //Arrange
            var userDetail = new UserDetail
            {
                FirstName = firstName,
                LastName = lastName,
            };
            var expectedResponse = _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.BadRequest,
                        Constants.InvalidUserRequestErrorMessage, Constants.InvalidUserRequestErrorCode);

            //Act
            var response = _service.UserRegistration(userDetail);

            //Assert
            Assert.Equivalent(expectedResponse, response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _logger.ReceivedCalls()
               .Select(call => call.GetArguments())
               .Count(arguments => !((LogLevel)arguments[0]).Equals(LogLevel.Error)
               || !((IReadOnlyList<KeyValuePair<string, object>>)arguments[2]).Last().Value.ToString()
               .Contains(Constants.InvalidUserRequestErrorLog)).Equals(1);
        }

        [Fact]
        public void Should_Return_Success_Response_When_User_Detail_Stored_Into_Json_File()
        {
            //Arrange
            var userDetail = new UserDetail
            {
                FirstName = "firstName",
                LastName = "lastName",
            };
            _fileProcessService.StoreUserDetailsIntoJsonFile(userDetail).Returns(true);
            var expectedResponse = _apiResponseFactory.CreateValidApiResponse();

            //Act
            var response = _service.UserRegistration(userDetail);

            //Assert
            Assert.Equivalent(expectedResponse, response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _logger.ReceivedCalls()
               .Select(call => call.GetArguments())
               .Count(arguments => !((LogLevel)arguments[0]).Equals(LogLevel.Error)
               || !((IReadOnlyList<KeyValuePair<string, object>>)arguments[2]).Last().Value.ToString()
               .Contains(Constants.UserRegistrationSuccessInfoLog)).Equals(1);
        }

        [Fact]
        public void Should_Return_InternalServer_Error_When_User_Detail_Is_Not_Stored_Into_Json_File()
        {
            //Arrange
            var userDetail = new UserDetail
            {
                FirstName = "firstName",
                LastName = "lastName",
            };
            _fileProcessService.StoreUserDetailsIntoJsonFile(userDetail).Returns(false);
            var expectedResponse = _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.InternalServerError,
                        Constants.UnhandledExceptionMessage, Constants.FileProcessErrorCode);

            //Act
            var response = _service.UserRegistration(userDetail);

            //Assert
            Assert.Equivalent(expectedResponse, response);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);          
        }

        [Fact]
        public void Should_Return_InternalServer_Error_When_File_Processor_Thrown_Exception()
        {
            //Arrange
            var userDetail = new UserDetail
            {
                FirstName = "firstName",
                LastName = "lastName",
            };
            _fileProcessService.StoreUserDetailsIntoJsonFile(userDetail).Throws(new Exception());
            var expectedResponse = _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.InternalServerError,
                    Constants.UnhandledExceptionMessage, Constants.UnhandledExceptionErrorCode);

            //Act
            var response = _service.UserRegistration(userDetail);

            //Assert
            Assert.Equivalent(expectedResponse, response);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            _logger.ReceivedCalls()
              .Select(call => call.GetArguments())
              .Count(arguments => !((LogLevel)arguments[0]).Equals(LogLevel.Error)
              || !((IReadOnlyList<KeyValuePair<string, object>>)arguments[2]).Last().Value.ToString()
              .Contains(Constants.UnhandledExceptionErrorLog)).Equals(1);
        }

    }
}
