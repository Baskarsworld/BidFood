using Bidfood.Common;
using Bidfood.Infrastructure;
using Bidfood.Models;
using System.Net;

namespace Bidfood.Services
{
    public class UserRegistrationService: IUserRegistrationService
    {
        private readonly ILogger<UserRegistrationService> _logger;
        private readonly IFileProcessService _fileProcessService;
        private readonly IApiResponseFactory _apiResponseFactory;        

        public UserRegistrationService(ILogger<UserRegistrationService> logger,
            IFileProcessService fileProcessService,
            IApiResponseFactory apiResponseFactory)
        {
            _logger = logger;
            _fileProcessService = fileProcessService;
            _apiResponseFactory = apiResponseFactory;
        }

        public ApiResponse UserRegistration(UserDetail userDetail)
        {
            try
            {
                if (ValidateRequest(userDetail))
                {
                    _logger.LogError(Constants.InvalidUserRequestErrorLog);

                    return _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.BadRequest, 
                        Constants.InvalidUserRequestErrorMessage, Constants.InvalidUserRequestErrorCode);
                }

                var response = _fileProcessService.StoreUserDetailsIntoJsonFile(userDetail);
                if (response)
                {
                    _logger.LogInformation(Constants.UserRegistrationSuccessInfoLog);
                    return _apiResponseFactory.CreateValidApiResponse();
                }
                else
                {
                   return _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.InternalServerError,
                        Constants.UnhandledExceptionMessage, Constants.FileProcessErrorCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Constants.UnhandledExceptionErrorLog, ex);
                return _apiResponseFactory.CreateErrorApiResponse(HttpStatusCode.InternalServerError,
                    Constants.UnhandledExceptionMessage, Constants.UnhandledExceptionErrorCode);
            }
        }

        private bool ValidateRequest(UserDetail userDetail)
        {
            return userDetail == null || userDetail.FirstName.IsNullOrEmpty() || userDetail.LastName.IsNullOrEmpty();
        }
    }

    public interface IUserRegistrationService
    {
        public ApiResponse UserRegistration(UserDetail userDetail);
    }
}
