using System.Net;

namespace Bidfood.Infrastructure
{
    public class ApiResponseFactory : IApiResponseFactory
    {
        public ApiResponse CreateValidApiResponse()
        {
            return new ApiResponse
            { 
                StatusCode = HttpStatusCode.OK
            };
        }

        public ApiResponse CreateErrorApiResponse(HttpStatusCode statusCode, string errorMessage, string errorCode)
        {
            return new ApiResponse
            {
                StatusCode = statusCode,
                Error = new Error
                {
                    Message= errorMessage,
                    Code= errorCode
                }
            };
        }
    }

    public interface IApiResponseFactory
    {
        public ApiResponse CreateValidApiResponse();
        public ApiResponse CreateErrorApiResponse(HttpStatusCode statusCode, string errorMessage, string errorCode);

    }
}
